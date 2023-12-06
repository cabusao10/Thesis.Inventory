import { DefaultButton, DetailsList, DetailsListLayoutMode, PrimaryButton, SelectionMode, createTheme, initializeIcons, loadTheme } from "@fluentui/react";
import react, { Component } from "react";
import { Navigation } from "./Navigation";
import { PageHeader } from "./PageHeader";
import axios from "axios";
import YesNo from "./YesOrNoModal";


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

export default class Archives extends Component {

    constructor(props) {
        super(props)
        this.state = {
            allitems: [],
            categories: [],
            uomlist: [],
            archivedId:0,
            hidedeleteModal: true,
            hiderestoreModal: true
        }
        this.getAllArchives();
        this.getAllCategorys();
        this.getAllUOM();

        
    }
    options = {
        headers: {
            'Content-type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
        }
    }
    async getAllUOM() {
        await axios.get(process.env.REACT_APP_API_URL + `Product/GetUom`, this.options)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    var rawUom = response.data.map(
                        function (uom) {
                            return {
                                key: uom.id,
                                text: uom.name
                            }
                        }
                    )

                    this.setState({ uomlist: rawUom });
                }
                else {
                    console.log(response.message);
                }
            });
    }

    getAllCategorys = async () => {

        await axios.get(process.env.REACT_APP_API_URL + `Product/GetCategories`, this.options)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    var rawcategories = response.data.map(
                        function (cat) {

                            return {
                                key: cat.id,
                                text: cat.name
                            };
                        }
                    )
                    this.setState({ categories: rawcategories })
                }
                else {
                    console.log(response.message);
                }
            })
    }

    handleDelete = (id) => {

    }

    handleColumnRender = (item, index, column) => {
        if (column.fieldName === 'actions') {
            return <><PrimaryButton onClick={() => this.setState({ hiderestoreModal: false , archivedId:item.id})} style={{ marginRight: 10 }}>Restore</PrimaryButton>
                <DefaultButton onClick={() => this.setState({ hidedeleteModal: false , archivedId:item.id})}>Delete</DefaultButton></>
        }
        else if (column.fieldName === 'categoryId') {
            const tmpCategory = this.state.categories.filter((category) => item.categoryId == category.key)[0];
            if (tmpCategory === undefined) return <></>;
            return <>{tmpCategory.text}</>
        }
        else if (column.fieldName === 'uomId') {
            const tmpuom = this.state.uomlist.filter((uom) => item.uomId == uom.key)[0];
            if (tmpuom === undefined) return <></>;
            return <>{tmpuom.text}</>
        }
        return item[column.fieldName];
    }

    getAllArchives = async () => {
        await axios.get(process.env.REACT_APP_API_URL + `Product/GetArchives?page=${this.state.currentpage ?? 1}`, this.options)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    this.state.currentpage = response.data.currentpage;
                    this.state.pagecount = response.data.pagecount;
                    this.setState({ allitems: response.data.data })
                }
                else {
                    console.log(response.message);
                }

            });
    }

    handleDeleteAnswer = async (e) => {
        this.setState({ isYesNoHidden: true })
        if (e === 'Yes') {
            this.setState({ allitems: this.state.allitems.filter((item) => item.id !== this.state.archivedId) })
            console.log(this.state.archivedId);
            var request = {
                productId: this.state.archivedId
            }
            await axios.put(process.env.REACT_APP_API_URL + `Product/Delete`, request, this.options)
                .then(res => {
                    var response = res.data;
                    if (response.succeeded) {
                        alert(response.message);
                        this.setState({hidedeleteModal:true});
                    }
                    else {
                        console.log(response.message);
                    }
                });
        }
    }

    handleRestoringAnswer = async (e) => {
        this.setState({ isYesNoHidden: true })
        if (e === 'Yes') {
            this.setState({ allitems: this.state.allitems.filter((item) => item.id !== this.state.archivedId) })
            console.log(this.state.archivedId);
            var request = {
                productId: this.state.archivedId
            }
            await axios.put(process.env.REACT_APP_API_URL + `Product/Restore`, request, this.options)
                .then(res => {
                    var response = res.data;
                    if (response.succeeded) {
                        alert(response.message);
                        this.setState({hiderestoreModal:true});
                    }
                    else {
                        console.log(response.message);
                    }
                });
        }
    }

    columns = [{ key: 'column1', name: 'Id', fieldName: 'id', minWidth: 20, maxWidth: 20 },
    { key: 'column2', name: 'Product Name', fieldName: 'productName', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column3', name: 'Product Id', fieldName: 'productId', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column4', name: 'Category', fieldName: 'categoryId', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column5', name: 'UOM', fieldName: 'uomId', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column6', name: 'Price', fieldName: 'price', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column7', name: 'Quantity', fieldName: 'quantity', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column8', name: 'Actions', fieldName: 'actions', minWidth: 200, maxWidth: 200, isResizable: true }]

    render() {
        loadTheme(myTheme);
        initializeIcons();
        return (
            <div className='mainbg'>
                <div style={{ height: '100%' }} className="sidebar">
                    <Navigation selectedNav='key6' />
                </div>
                <div className="frame" >
                    <YesNo message='Do you want to continue restoring the product?' hideModal={(this.state.hiderestoreModal)} onAnswer={this.handleRestoringAnswer} />
                    <YesNo message='Do you want to continue deleting the product?' hideModal={(this.state.hidedeleteModal)} onAnswer={this.handleDeleteAnswer} />
                    <PageHeader title='Archived list' />
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

            </div>)
    }
}