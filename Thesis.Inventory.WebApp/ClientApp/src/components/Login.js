import React, { Component } from 'react';
import axios from 'axios';
import background from '../images/inventorybg.jpg'
import { PrimaryButton, TextField, createTheme, loadTheme } from '@fluentui/react';

const myTheme = createTheme({
    palette: {
      themePrimary: '#43b480',
      themeLighterAlt: '#f6fcf9',
      themeLighter: '#dbf3e8',
      themeLight: '#bde9d4',
      themeTertiary: '#83d3ad',
      themeSecondary: '#55be8d',
      themeDarkAlt: '#3ca373',
      themeDark: '#338a61',
      themeDarker: '#266548',
      neutralLighterAlt: '#faf9f8',
      neutralLighter: '#f3f2f1',
      neutralLight: '#edebe9',
      neutralQuaternaryAlt: '#e1dfdd',
      neutralQuaternary: '#d0d0d0',
      neutralTertiaryAlt: '#c8c6c4',
      neutralTertiary: '#bab8b7',
      neutralSecondary: '#a3a2a0',
      neutralSecondaryAlt: '#a3a2a0',
      neutralPrimaryAlt: '#8d8b8a',
      neutralPrimary: '#323130',
      neutralDark: '#605e5d',
      black: '#494847',
      white: '#ffffff',
    }});
export class Login extends Component {

    static displayName = Login.name;

    constructor(props) {
        super(props);
        this.state = { username: "", password: "" };
    }

    handleUsername = e => {
        e.preventDefault();

        this.setState({ username: e.target.value });
    }

    handlePassword = e => {
        e.preventDefault();
        this.setState({ password: e.target.value });
    }
    handleLogin = e => {
        e.preventDefault();

        const userrequest = {
            Username: this.state.username,
            Password: this.state.password
        }

        axios.post(process.env.REACT_APP_API_URL + "User/Login", userrequest)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    var data = response = response.data;
                    console.log(data);
                    localStorage.setItem("token", data.bearerToken);
                    localStorage.setItem("username", data.username);
                    localStorage.setItem("email", data.email);
                    localStorage.setItem("id", data.id);
                    localStorage.setItem("businessname", data.businessName);
                    localStorage.setItem("_66Dsd87fhH", data.role);

                    
                    if(data.status == 1){
                        window.location.href = '/dashboard';
                    }
                    else{
                        window.location.href = '/validate';
                    }

                 
                
            
                }
                else {
                    alert(response.message);
                }

            });
    }
    render() {
        loadTheme(myTheme)
        return (
            <div className='mainbg' style={{ backgroundImage: `url(${background})` }}>
                <div class="center">
                    <h1>Login</h1>
                    <form method="post" onSubmit={this.handleLogin}>
                        <div class="txt_field">
                            <TextField label='Username' name='username' onChange={this.handleUsername} />

                        </div>
                        <div class="txt_field">
                            <TextField
                                label="Password"
                                type="password"
                                canRevealPassword
                                revealPasswordAriaLabel="Show password"
                                onChange={this.handlePassword}
                            />
                        </div>
                        <div class="pass">Forgot Password?</div>
                        <PrimaryButton type='submit'>Login</PrimaryButton>
                        <div class="signup_link">
                            Not a member? <a href="/register">Signup</a>
                        </div>
                    </form>
                </div>
            </div>
        );
    }
}