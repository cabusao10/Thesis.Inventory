import React, { Component } from "react";
import { Navigation } from "./Navigation";
import { CompoundButton, DetailsList, DetailsListLayoutMode, PrimaryButton, SelectionMode, createTheme, loadTheme } from "@fluentui/react";
import {
    ChartDataMode,
    HorizontalBarChart,
    IChartProps,
    IHorizontalBarChartProps,
    DataVizPalette,
    getColorFromToken,
} from '@fluentui/react-charting';
import axios from "axios";
import { Label } from "reactstrap";
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
        this.state = {
            salesToday: 0,
            monthSales: 0,
            totalSales: 0,
            data: [],
            lowstockData: [],
        }
        this.getDetails();
        this.getLowStocks();

    }

    getLowStocks = () => {
        axios.get(process.env.REACT_APP_API_URL + "Dashboard/LowStocks")
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    this.setState({ lowstockData: response.data })
                }
            });
    }
    handleExport = (type) => {
        var handle = window.open(process.env.REACT_APP_API_URL + "dashboard/Reports?type=" + type, "_blank");
        handle.blur();
        window.focus()
      
    }
    getDetails = () => {
        axios.get(process.env.REACT_APP_API_URL + "Dashboard/details")
            .then(res => {
                var response = res.data;
                if (response.succeeded) {

                    var dash = response.data;
                    console.log(dash);
                    var totalsoldArray = dash.top5.map(
                        function (item) {
                            return item.totalSold
                        }
                    )
                    var totalProductSold = totalsoldArray.reduce((a, c) => a + c, 0);
                    console.log(totalsoldArray)
                    var tmpdata = dash.top5.map(
                        function (item) {

                            return {
                                chartTitle: <><b>{item.productId}</b><Label style={{ marginLeft: 5 }}>{item.productName}</Label></>,
                                chartData: [
                                    {
                                        legend: item.productName,
                                        horizontalBarChartdata: { x: item.totalSold, y: totalProductSold },
                                        color: '#00425A',
                                        xAxisCalloutData: '',
                                        yAxisCalloutData: item.totalSold,
                                    },
                                ],
                            }
                        }
                    )


                    this.setState({
                        salesToday: dash.totalSalesToday,
                        monthSales: dash.totalSalesThisMonth,
                        totalSales: dash.totalSales,
                        data: tmpdata,
                    })
                }
                else {
                    alert(response.message);
                }

            });
    }

    columns = [{ key: 'column1', name: 'Product Id', fieldName: 'productId', minWidth: 100, maxWidth: 100 },
    { key: 'column2', name: 'Product Name', fieldName: 'productName', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column3', name: 'Quantity', fieldName: 'quantity', minWidth: 100, maxWidth: 200, isResizable: true },
    ]
    render() {
        loadTheme(myTheme);
        return (
            <div className='mainbg'>
                <div style={{ height: '100%' }} className="sidebar">
                    <Navigation selectedNav='key1' />
                </div>
                <div className="frame" >
                    <table style={{ width: '100%' }}>
                        <tr>
                            <td>
                                <div class="container--card">
                                    <h3 class="main--title">Sales</h3>
                                    <div class="card--wrapper">
                                        <div class="payment--card light-red" >
                                            <div class="card--header">
                                                <div class="amount">
                                                    <span class="title">Total Sales today</span>
                                                    <span class="amount-value"> ₱{this.state.salesToday}</span>
                                                </div>

                                            </div>

                                        </div>
                                        <div class="payment--card light-green" >
                                            <div class="card--header">
                                                <div class="amount">
                                                    <span class="title">Current Month Total Sales</span>
                                                    <span class="amount-value"> ₱{this.state.monthSales}</span>
                                                </div>

                                            </div>

                                        </div>
                                        <div class="payment--card light-blue" >
                                            <div class="card--header">
                                                <div class="amount">
                                                    <span class="title">Total Sales</span>
                                                    <span class="amount-value">₱{this.state.totalSales}</span>
                                                </div>

                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div class="tabular--wrapper">
                                    <div className="card--wrapper">
                                        <div className="dashboard-card">
                                            <Label style={{ color: '#00425A' }} >Top 5 Product</Label>
                                            <HorizontalBarChart

                                                culture={window.navigator.language}
                                                data={this.state.data}
                                                hideRatio={true}
                                            />
                                        </div>
                                        <div className="dashboard-card stocks">
                                            <Label style={{ color: '#00425A' }} >Low stock products</Label>
                                            <DetailsList

                                                items={this.state.lowstockData}
                                                columns={this.columns}
                                                setKey="key"
                                                compact={false}
                                                isHeaderVisible={true}
                                                selectionMode={SelectionMode.none}
                                                layoutMode={DetailsListLayoutMode.justified}
                                                selectionPreservedOnEmptyClick={true}
                                                ariaLabelForSelectionColumn="Toggle selection"
                                                ariaLabelForSelectAllCheckbox="Toggle selection for all items"
                                                checkButtonAriaLabel="select row"
                                                theme={myTheme}
                                            />
                                        </div>
                                    </div>

                                </div>
                                <div class="tabular--wrapper">
                                    <h3 class="main--title">Available Reports</h3>
                                    <div className="card--wrapper">

                                        <CompoundButton secondaryText="Export a Sales report."  onClick={()=> this.handleExport(2)}>
                                            Sales Report
                                        </CompoundButton>
                                        <CompoundButton secondaryText="Export an Users report." onClick={()=> this.handleExport(1)}>
                                            Users Report
                                        </CompoundButton>
                                        <CompoundButton secondaryText="Export an Orders report." onClick={()=> this.handleExport(0)}>
                                            Orders Report
                                        </CompoundButton>
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