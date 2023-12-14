import { Nav, INavLink, INavStyles, INavLinkGroup } from '@fluentui/react/lib/Nav';
import { useHistory, useNavigate } from 'react-router';

import React, { Component } from "react";
import { initializeIcons } from '@fluentui/react/lib/Icons';
import { createTheme, loadTheme } from '@fluentui/react';
import { useSearchParams } from 'react-router-dom';
import axios from 'axios';

const myTheme = createTheme({
    palette: {
        themePrimary: '#8ea1e1',
        themeLighterAlt: '#060609',
        themeLighter: '#171a24',
        themeLight: '#2a3043',
        themeTertiary: '#556087',
        themeSecondary: '#7c8dc5',
        themeDarkAlt: '#98a9e3',
        themeDark: '#a7b6e8',
        themeDarker: '#bdc8ee',
        neutralLighterAlt: '#004057',
        neutralLighter: '#003f55',
        neutralLight: '#003c52',
        neutralQuaternaryAlt: '#00384c',
        neutralQuaternary: '#003549',
        neutralTertiaryAlt: '#003346',
        neutralTertiary: '#c8c8c8',
        neutralSecondary: '#d0d0d0',
        neutralSecondaryAlt: '#d0d0d0',
        neutralPrimaryAlt: '#dadada',
        neutralPrimary: '#ffffff',
        neutralDark: '#f4f4f4',
        black: '#f8f8f8',
        white: '#00425a',
    }
});

const navLinkGroups: INavLinkGroup[] = [
    {
        links: [
            {
                name: 'Dashboard',
                icon: 'ViewDashboard',
                key: 'key1',
                link: '/dashboard',
                target: '_blank',
            },
            {
                name: 'Orders',
                link: '/orders',
                icon: 'ReservationOrders',
                key: 'key2',
                target: '_blank',
            },
            {
                name: 'Inventory',
                link: '/inventory?page=1',
                icon: 'StackedColumnChart2Fill',
                key: 'key3',
                target: '_blank',
            },
            {
                name: 'Archive',
                link: '/archives',
                icon: 'Archive',
                key: 'key6',
                target: '_blank',
            },
            {
                name: 'User Management',
                link: '/users',
                icon: 'FabricUserFolder',
                key: 'key7',
                target: '_blank',
            },
            {
                name: 'Messages',
                link: '/chat',
                icon: 'Chat',
                key: 'key10',
                target: '_blank',
            },
            {
                name: 'Logout',
                link: '/',
                icon: 'SignOut',
                key: 'key9',
                target: '_blank',
            },
        ],
    },
];

const navStyles: Partial<INavStyles> = {
    root: {
        backgroundColor: '#00425A',
        boxSizing: 'border-box',
        overflowY: 'auto',
    },
    link: {
        height: '58px',
        color: 'white',
        fontSize: '16px'
    },
};


export class Navigation extends Component {

    componentDidMount(){
        this.validateSession();
    }
    constructor(props) {
        super(props);
        this.state = { selectedKey: props.selectedNav }
    }
    options = {
        headers: {
            'Content-type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
        }
    }
    validateSession= async ()=>{
        await axios.post(process.env.REACT_APP_API_URL + `User/Validate`,'',  this.options)
        .then(res => {
            var response = res.data;
            if (response) {
              
            }
            else {
                window.location.href = '/';
            }
        })
        .catch(err=> 
            window.location.href = '/')
    }

    handleNavClick = (e, item) => {

    }
    render() {

        initializeIcons();
        return (
            <MyComponent selectedKey={this.state.selectedKey} />
        )
    }
}
function MyComponent(state) {
    const history = useNavigate();

    function handleNavClick(e, i) {
        if (i.name === 'Signout') {
            localStorage.clear();
        }
        history(i.link);
    }

    return (
        <div style={{ height: '100%', border: '1px solid #eee', backgroundColor: '#00425A' }}>
            <Nav
                theme={myTheme}
                selectedKey={state.selectedKey}
                ariaLabel="Nav basic example"
                styles={navStyles}
                groups={navLinkGroups}
                onLinkClick={handleNavClick}
            />
        </div>
    );
}