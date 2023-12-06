import React, { Component } from "react";
import { Navigation } from "./Navigation";
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
export class Dashboard extends Component {
    static displayname = Dashboard.name;

    constructor(props) {
        super(props);
    }


    render() {
        loadTheme(myTheme);
        return (
            <div className='mainbg'>
                <div style={{ height: '100%' }} className="sidebar">
                    <Navigation selectedNav='key1'/>
                </div>
                <div className="frame" >
                    <table style={{ width: '100%' }}>
                        <tr>
                            <td>
                                <div class="container--card">
                                    <h3 class="main--title">Today's Sale</h3>
                                    <div class="card--wrapper">
                                        <div class="payment--card light-red" >
                                            <div class="card--header">
                                                <div class="amount">
                                                    <span class="title">Payment Amount</span>
                                                    <span class="amount-value"> ₱500.00</span>
                                                </div>
                                                <i class="fa-solid fa-peso-sign icon"></i>
                                            </div>
                                            <span class="card--detail">**** **** **** 2406</span>
                                        </div>
                                        <div class="payment--card light-green" >
                                            <div class="card--header">
                                                <div class="amount">
                                                    <span class="title">Payment Order</span>
                                                    <span class="amount-value"> ₱69.00</span>
                                                </div>
                                                <i class="fa-solid fa-bag-shopping icon"></i>
                                            </div>
                                            <span class="card--detail">**** **** **** 1851</span>
                                        </div>
                                        <div class="payment--card light-blue" >
                                            <div class="card--header">
                                                <div class="amount">
                                                    <span class="title">Payment Proceed</span>
                                                    <span class="amount-value">₱360.00</span>
                                                </div>
                                                <i class="fa-solid fa-check icon"></i>
                                            </div>
                                            <span class="card--detail">**** **** **** 6969</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="tabular--wrapper">
                                    <h3 class="main--title">Latest transactions</h3>
                                    <div class="table-container">
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th>Date</th>
                                                    <th>Transaction Type</th>
                                                    <th>Description</th>
                                                    <th>Amount</th>
                                                    <th>Category</th>
                                                    <th>Status</th>
                                                    <th>Action</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>2023-09-27</td>
                                                    <td>Expenses</td>
                                                    <td>Hardware Supplies</td>
                                                    <td>$260</td>
                                                    <td>Hardware Expenses</td>
                                                    <td>Pending</td>
                                                    <td><button>Edit</button></td>
                                                </tr>
                                                <tr>
                                                    <td>2023-09-27</td>
                                                    <td>Income</td>
                                                    <td>Hardware Supplies</td>
                                                    <td>$200</td>
                                                    <td>Hardware Expenses</td>
                                                    <td>Pending</td>
                                                    <td><button>Edit</button></td>
                                                </tr>
                                            </tbody>
                                            <tfoot>
                                                <tr>
                                                    <td colspan="7">Total: $2,690.00</td>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

            </div>

        )
    }
}