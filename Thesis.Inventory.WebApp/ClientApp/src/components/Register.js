import { Calendar, ChoiceGroup, ComboBox, DatePicker, DefaultButton, PrimaryButton, TextField, createTheme, initializeIcons, loadTheme } from '@fluentui/react';
import React, { Component } from 'react';
import background from '../images/inventorybg.jpg'
import axios from 'axios';

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
    }
});
const options: IChoiceGroupOption[] = [
    { key: '0', text: 'Male' },
    { key: '1', text: 'Female' },
    { key: '2', text: 'Prefer not to say' },

];
export class Register extends Component {

    static displayName = Register.name;

    fetchProvince = () => {
        axios.get(process.env.REACT_APP_API_URL + "User/GetProvinces")
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    this.setState({ provinces: response.data })
                }
                else {
                    alert(response.message);
                }

            });
    }
    constructor(props) {
        super(props);
        this.state = {
            provinces: [],
            fullname: '',
            businessName: '',
            username: '',
            province: '',
            address: '',
            zipCode: '',
            email: '',
            phoneNumber: '',
            password: '',
            confirmPassword: '',
            gender: 0,
            birthdate: '',
            maxDate: new Date(),
        }
        this.fetchProvince();
        this.setMaxDate();
    }

    setMaxDate() {
        var curdata = new Date();
        curdata.setFullYear(curdata.getFullYear() - 18);

        this.setState({ maxDate: curdata });
        
    }
    handleRegister = e => {
        e.preventDefault();

        var registerRequest = {
            username: this.state.username,
            password: this.state.password,
            confirmPassword: this.state.confirmPassword,
            email: this.state.email,
            fullname: this.state.fullname,
            contactNumber: this.state.contactNumber,
            address: this.state.address,
            zipCode: this.state.zipCode,
            provinceId: parseInt(this.state.province),
            gender: parseInt(this.state.gender),
            birthdate: this.state.birthdate,
            businessName: this.state.businessName,
            role: 3,

        }
        console.log(registerRequest)
        axios.post(process.env.REACT_APP_API_URL + "User/Register", registerRequest)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    alert("Success registering an account.")
                    window.location.href = '/validate';
                }
                else {
                    alert(response.message);
                }

            });
    }
    render() {
        loadTheme(myTheme);
        initializeIcons();
        const test = [{ key: '1', text: 'test' },
        { key: '2', text: 'test2' }];
        return (
            <div className='mainbg' style={{ backgroundImage: `url(${background})` }}>
                <div class="center">
                    <div class="title">Tell us about your business</div>
                    <p1>For the purpose of transparency, your details are required</p1>
                    <form action="" method="POST">
                        <div class="user-details">
                            <div class="input-box">
                                <TextField label='Full name' required placeholder='Enter your full name' onChange={e => { this.setState({ fullname: e.target.value }) }} />
                            </div>
                            <div class="input-box">
                                <TextField label='Business Name' required placeholder='Enter your business name'
                                    onChange={e => { this.setState({ businessName: e.target.value }) }} />

                            </div>
                            <div class="input-box">
                                <DatePicker label='Birthdate' required placeholder='Enter your birthdate'
                                    maxDate={this.state.maxDate}
                                    onSelectDate={e => { this.setState({ birthdate: e }) }} />

                            </div>
                            <div class="input-box">
                                <TextField label='Username' required placeholder='Enter your username'
                                    onChange={e => { this.setState({ username: e.target.value }) }} />
                            </div>
                            <div class="input-box">
                                <ComboBox label='Province' options={this.state.provinces} required
                                    onItemClick={(o, i, val) => { this.setState({ province: i.key }) }}
                                    onChange={(o, i, val) => { this.setState({ province: i.key }) }} />
                            </div>
                            <div class="input-box">
                                <TextField label='Address' required placeholder='Enter your address'
                                    onChange={e => { this.setState({ address: e.target.value }) }} />
                            </div>
                            <div class="input-box">
                                <TextField label='Zip Code' required placeholder='Enter your Zip Code'
                                    onChange={e => { this.setState({ zipCode: e.target.value }) }} />
                            </div>
                            <div class="input-box">
                                <TextField label='Email' type='email' required placeholder='Enter your email'
                                    onChange={e => { this.setState({ email: e.target.value }) }} />
                            </div>
                            <div class="input-box">
                                <TextField label='Phone Number' required placeholder='Enter your phone number'
                                    onChange={e => { this.setState({ contactNumber: e.target.value }) }} />
                            </div>
                            <div class="input-box">
                                <TextField type='password' label='Password' required placeholder='Enter your password'
                                    onChange={e => { this.setState({ password: e.target.value }) }} />
                            </div>
                            <div class="input-box">
                                <TextField type='password' label='Confirm Password' required placeholder='Confirm password'
                                    onChange={e => { this.setState({ confirmPassword: e.target.value }) }} />

                            </div>
                        </div>
                        <div class="gender-details">
                            <ChoiceGroup options={options} label='Gender' onChange={(e, option) => { this.setState({ gender: option.key }) }} />

                            <div class="registrationbuttons">
                                <PrimaryButton type='submit' onClick={this.handleRegister}>Register</PrimaryButton>
                                <DefaultButton style={{ left: 10 }} type='submit' onClick={() => { window.location.href = '/login'; }}>Back to login</DefaultButton>
                            </div>


                        </div>
                    </form>
                </div>
            </div>
        );
    }
}