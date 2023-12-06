import react, { Component } from "react";
import { Navigation } from "./Navigation";
import { Dashboard } from "./Dashboard";
import { Orders } from "./Orders";
import { createTheme, loadTheme } from "@fluentui/react";


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
export class Main extends Component {
    static displayname = Main.name;
    constructor(props) {
        super(props);
    }

    render() {
        loadTheme(myTheme);
        return (
            <div className='mainbg'>
                <div style={{ height: '100%' }} className="sidebar">
                    <Navigation />
                </div>
                <div className="frame" >
                    <Orders />
                </div>

            </div>
        )
    }
}