import { DefaultButton, DetailsList, DetailsListLayoutMode, Label, PrimaryButton, SearchBox, SelectionMode, createTheme, initializeIcons, loadTheme } from "@fluentui/react";
import react, { Component } from "react";
import { Navigation } from "./Navigation";
import { PageHeader } from "./PageHeader";
import { useSearchParams } from "react-router-dom";
import axios from "axios";
import YesNo from "./YesOrNoModal";
import { Pagination } from "./Pagination";


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
const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);

const menuProps: IContextualMenuProps = {
    items: [
        {
            key: 'delete',
            text: 'Preparing',
            iconProps: { iconName: 'Delete' },
        },

    ],
};
export class Orders extends Component {



    constructor(props) {
        super(props)
        this.state = {
            allitems: [],
            pagecount: 0,
            currentpage: urlParams.get('page') ?? 1,
            isModalOpen: false,
            categories: [],
            productId: null,
            productName: '',
            price: 0,
            category: 0,
            UOM: 0,
            quantity: 0,
            uomlist: [],
            isYesNoHidden: true,
            archivedId: 0,
            selectedFile: null,
            isModalOpenEdit: false,
            imageData: null,
            id: 0,
            productSample: null,
            isYesNoHidden: true,
            orderId: 0,
            newStatus: 0,
        }

        this.getAllItems();

    }

    options = {
        headers: {
            'Content-type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
        }
    }

    getAllItems = async () => {
        await axios.get(process.env.REACT_APP_API_URL + `Order/GetAll?page=${this.state.currentpage ?? 1}`, this.options)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    this.state.currentpage = response.data.currentpage;
                    this.state.pagecount = response.data.pagecount;
                    this.setState({ allitems: response.data.data, productSample: response.data.data[0].product })
                    console.log(response.data.data)

                }
                else {
                    console.log(response.message);
                }

            })
            .catch(error => {
                console.log(error);
            });
    }
    columns = [{ key: 'column1', name: 'Id', fieldName: 'id', minWidth: 50, maxWidth: 50, isResizable: true },
    { key: 'column2', name: 'Product Name', fieldName: 'productName', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column3', name: 'Customer Name', fieldName: 'customerName', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column4', name: 'Phone Number', fieldName: 'phoneNumber', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column5', name: 'Address', fieldName: 'address', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column6', name: 'Quantity', fieldName: 'quantity', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column7', name: 'Date Ordered', fieldName: 'dateOrdered', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column8', name: 'Status', fieldName: 'status', minWidth: 100, maxWidth: 100, isResizable: true },
    { key: 'column8', name: '', fieldName: 'action', minWidth: 200, maxWidth: 200, isResizable: true }]

    handleColumnRender = (item, index, column) => {
        if (column.fieldName === 'status') {
            return <>
                {item.status === 0 && <Label style={{ color: '#F7CB73' }}>Pending</Label>}
                {item.status === 1 && <Label style={{ color: '#9DB9D1' }}>Paid</Label>}
                {item.status === 2 && <Label style={{ color: '#D9512C' }}>Cancelled</Label>}
                {item.status === 3 && <Label style={{ color: '#1F586A' }}>Packed</Label>}
                {item.status === 4 && <Label style={{ color: '#00B0E5' }}>Shipped</Label>}
                {item.status === 5 && <Label style={{ color: '#3CA373' }}>Delivered</Label>}

            </>
        }
        else if (column.fieldName === 'action') {
            
            if (item.status === 1) {
                return <><PrimaryButton onClick={() => this.setState({ orderId: item.id, isYesNoHidden: false, newStatus: 3 })}>Change to Packed</PrimaryButton></>
            }
            else if (item.status === 3) {
                return <><PrimaryButton onClick={() => this.setState({ orderId: item.id, isYesNoHidden: false, newStatus: 4 })}>Change to Shipped</PrimaryButton></>
            }
            else if (item.status === 4) {
                return <><PrimaryButton  onClick={() => this.setState({ orderId: item.id, isYesNoHidden: false, newStatus: 5 })}>Change to Delivered</PrimaryButton></>
            }
            return <>
            </>
        }

        return item[column.fieldName];
    }

    handleChangeStatus = async (e) => {
        this.setState({ isYesNoHidden: true })
        if (e === 'Yes') {
            var request = {
                OrderId: this.state.orderId,
                NewStatus: this.state.newStatus,
            }

            await axios.patch(process.env.REACT_APP_API_URL + `Order/ChangeStatus`, request, this.options)
                .then(res => {
                    var response = res.data;
                    if (response.succeeded) {
                        this.getAllItems();
                    }
                    else {

                    }
                });
        }

    }
    handleOnSearch = async (newvalue) => {
        if (newvalue === '') {
            this.getAllItems();
            return;
        }
        await axios.get(process.env.REACT_APP_API_URL + `Order/GetById?id=${newvalue}&page=${this.state.currentpage ?? 1}`, this.options)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    console.log(response)
                    this.setState({
                        
                        currentpage: response.data.currentpage,
                        pagecount: response.data.pagecount,
                        allitems: response.data.data
                    })
                }
                else {
                    console.log(response.message);
                }

            });
    }
    render() {

        loadTheme(myTheme);
        initializeIcons();
        return (
            <div className='mainbg'>
                <YesNo message='Do you want to continue changing the status?' hideModal={(this.state.isYesNoHidden)} onAnswer={this.handleChangeStatus} />
                <div style={{ height: '100%' }} className="sidebar">
                    <Navigation selectedNav='key2' />
                </div>
                <div className="frame" >
                    <PageHeader title='Orders list' />
                 
                    <div className="product-menu">
                        <div style={{ display: 'flex' }}>
                            <div className="divsearch">
                                <SearchBox placeholder="Search using order id"
                                onSearch={this.handleOnSearch}
                                onClear={()=> this.handleOnSearch('')}
                                />
                            </div>
                          
                        </div>
                    </div>
                    <DetailsList
                        onRenderItemColumn={this.handleColumnRender}
                        items={this.state.allitems}
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
                <Pagination pagecount={this.state.pagecount} currentpage={this.state.currentpage} page='order' />
            </div>
        )
    }
}

export default Orders