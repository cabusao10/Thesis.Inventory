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
    }
});

export class Validation extends Component {
    constructor(props) {
        super(props);

        this.state = { VerificationCode: '' };
    }

    options = {
        headers: {
            'Content-type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
        }
    }

    handleValidation = e => {
        e.preventDefault();

        const request = {
            OTP: this.state.VerifcationCode,
        }

        console.log(request);
        axios.post(process.env.REACT_APP_API_URL + "User/Verify", request,this.options)
            .then(res => {
                var response = res.data;
             
                if (response) {
                    var data = response = response.data;
  

                    window.location.href = '/dashboard';




                }
                else {
                    alert(response.message);
                }

            });
    }
    render() {
        return (<div className='mainbg' style={{ backgroundImage: `url(${background})` }}>
            <div class="center">
                <h1>Validate your account</h1>
                <p>Check your email for validation code.</p>
                <form method="post" onSubmit={this.handleValidation}>
                    <div class="txt_field">
                        <TextField label='Validation Code'  onChange={(e) => this.setState({ VerifcationCode: e.target.value })} />

                    </div>

                    <PrimaryButton style={{ marginTop: 5 }} type='submit'>Validate</PrimaryButton>
                    {/* <div class="signup_link" style={{ marginTop: 10 }}>
                        <a href="/register">Resend code</a>
                    </div> */}
                </form>
            </div>
        </div>)
    }
}

