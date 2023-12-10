import React, { Component, useEffect, useState } from "react";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { CompoundButton, IconButton, Label, PrimaryButton, Stack, StackItem, TextField } from "@fluentui/react";
import { Navigation } from "./Navigation";
import { PageHeader } from "./PageHeader";
import axios from "axios";

export class Message extends Component {

    options = {
        accessTokenFactory: () => `${localStorage.getItem('token')}`,
    };
    constructor(props) {
        super(props);

        this.state = {
            connection: new HubConnectionBuilder()
                .withUrl(process.env.REACT_APP_CHAT_HUB_URL, this.options)
                .withAutomaticReconnect()
                .build(),
            connectionId: '',
            chat: '',
            messages: [],
            contacts: [],
            targetusername: ''

        }
        this.loadContacts();
        this.start();
    }

    axiosoptions = {
        headers: {
            'Content-type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
        }
    }
    messagesEndRef = React.createRef();
    loadContacts = async () => {
        await axios.get(process.env.REACT_APP_API_URL + `Chat/ActiveContacts`, this.axiosoptions)
            .then(res => {
                var response = res.data;

                if (response.succeeded) {

                    this.setState({ contacts: response.data })
                }
                else {
                    console.log(response.message);
                }

            })
            .catch(error => {
                console.log(error);
            });
    }
    loadOldMessages = async (username) => {
        await axios.get(process.env.REACT_APP_API_URL + `Chat/GetMessages?user=${username}`, this.axiosoptions)
            .then(res => {
                var response = res.data;

                if (response.succeeded) {
                    var raw = response.data.map(
                        function (msg) {
                            var obj_msg = JSON.parse(msg.message);

                            obj_msg.IsYou = localStorage.getItem("username") == obj_msg.User;
                            return obj_msg;
                        }
                    )
                    this.setState({ messages: raw })
                }
                else {
                    console.log(response.message);
                }

            })
            .catch(error => {
                console.log(error);
            });
    }
    start = () => {
        if (this.state.connection) {
            this.state.connection
                .start()
                .then(() => {
                    this.state.connection.on("ReceiveMessage", (userid, message) => {

                        var msg = JSON.parse(message);
                        msg.IsYou = localStorage.getItem("username") == msg.User;
                        this.state.messages.push(msg);


                        this.setState({ messages: this.state.messages });


                        this.scrollToBottom();
                    });
                })
                .catch((error) => console.log(error));
        }
    }


    scrollToBottom = () => {
        this.messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' })
    }

    keyPress = async (e) => {

        if (e.keyCode == 13) {
            if (this.state.connection && this.state.chat) {
                var msg = {
                    User: localStorage.getItem("username"),
                    Message: this.state.chat,
                };
                this.setState({ chat: '' });
                await this.state.connection.send("SendMessage", this.state.targetusername, JSON.stringify(msg));


            }
        }
    }

    openChat = async (username) => {
        console.log(username);
        this.setState({ targetusername: username ,messages:[]})
        await this.loadOldMessages(username);
        this.messagesEndRef.current?.scrollToBottom()
    }
    render() {


        return (
            <div className='mainbg'>
                <div style={{ height: '100%' }} className="sidebar">
                    <Navigation selectedNav='key10' />
                </div>
                <div className="frame" >

                    <PageHeader title='Messages' />
                    <div className="chat-container">
                        <div className="chat-people">
                            <Stack tokens={sectionStackTokens}>
                                {
                                    this.state.contacts.map(
                                        c =>
                                            <StackItem align="center">
                                                <CompoundButton
                                                    onClick={() => this.openChat(c.username)}
                                                    secondaryText={c.fullname} style={{ width: 180 }}>
                                                    {c.username}
                                                </CompoundButton>
                                            </StackItem>
                                    )
                                }
                            </Stack>
                        </div>
                        <div style={{ overflow: 'auto' }} id="msgbody">
                            <Stack className="chat-body" >
                                {this.state.messages.map(chat =>
                                    <StackItem align={chat.IsYou ? 'end' : 'start'} >
                                        <div className={chat.IsYou == true ? 'chat-me' : 'chat-other'} ref={this.messagesEndRef}>
                                            <Label>{chat.User}</Label>
                                            <p>{chat.Message}</p>
                                        </div>
                                    </StackItem>
                                )}

                            </Stack>


                        </div>
                        <TextField
                            className="chat-send"

                            onKeyUp={this.keyPress}
                            iconProps={iconProps} value={this.state.chat}
                            onChange={(e) => this.setState({ chat: e.target.value })} />

                        {/* <PrimaryButton onClick={this.handleSendMessage}>Send Message</PrimaryButton> */}
                    </div>
                </div>

            </div>
        )
    }
}
const iconProps = { iconName: 'Send' };
const sectionStackTokens: IStackTokens = { childrenGap: 10 };