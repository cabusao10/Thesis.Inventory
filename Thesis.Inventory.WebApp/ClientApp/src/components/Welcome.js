import React, { Component } from 'react';
import { Button, PrimaryButton, createTheme, loadTheme } from '@fluentui/react';
import styles from '../css/Welcome.module.css'

import background from '../images/inventorybg.jpg'

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

export class Welcome extends Component {


    static displayName = Welcome.name;

    constructor(props) {
        super(props);

    }

    signInClicked = e => {
        e.preventDefault();
        window.location.href = '/login';
    }
    render() {

        loadTheme(myTheme);
        return (
            <div className={styles.mainbg} style={{backgroundImage:`url(${background})`}}>

                <div className={styles.center}>
                    <h1 class={styles.welcometxt}>Welcome to TrackIn Sales</h1>
                    <p class={styles.tagline}>"Efficiently Manage Your Inventory, Boost Your Business."</p>
                    <p class={styles.role}>What are you? :</p>
                    <div class={styles.buttoncontainer}>
                        <PrimaryButton onClick={this.signInClicked} >Sign In</PrimaryButton>
                    </div>
                    <div class={styles.signup_link}>
                        Not a member? <a href="registration.php">Signup</a>
                    </div>
                </div>
            </div>
        );
    }
}