import { Component, useState } from "react";
import { Navigation } from "./Navigation";
import { PageHeader } from "./PageHeader";
import { ComboBox, DefaultButton, DetailsList, DetailsListLayoutMode, FontIcon, FontWeights, IconButton, Image, Label, Modal, PrimaryButton, SearchBox, SelectionMode, TextField, TooltipHost, createTheme, getTheme, loadTheme, mergeStyles } from "@fluentui/react";
import { Pagination } from "./Pagination";
import axios from "axios";
import { Link } from "react-router-dom";
import { faL } from "@fortawesome/free-solid-svg-icons";
import YesNo from "./YesOrNoModal";
import { useReactToPrint } from 'react-to-print';

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

export class Inventory extends Component {


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
            minimumQuantity: 0,
            uomlist: [],
            isYesNoHidden: true,
            archivedId: 0,
            selectedFile: null,
            isModalOpenEdit: false,
            imageData: null,
            id: 0,

        }
        this.getAllItems();
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

    handleAddNewProduct = async () => {
        const request = {
            productName: this.state.productName,
            productId: this.state.productId,
            categoryId: this.state.category,
            uOMId: this.state.UOM,
            quantity: this.state.quantity,
            price: this.state.price,
            productImage: this.state.selectedFile,
            minimumQuantity: this.state.minimumQuantity
        }

        console.log(request);
        await axios.post(process.env.REACT_APP_API_URL + `Product/AddProduct`, request, this.options)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    alert(response.message);
                    this.setState({ isModalOpen: false })
                }
                else {
                    alert(response.message);
                }
            })
    }
    handleEdit = async (id) => {
        await axios.get(process.env.REACT_APP_API_URL + `Product/GetById?id=${id}`, this.options)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    var product = response.data;
                    console.log(response);
                    this.setState({
                        productName: product.productName,
                        productId: product.productId,
                        category: product.categoryId,
                        UOM: product.uomId,
                        quantity: product.quantity,
                        price: product.price,
                        imageData: product.image,
                        imagetype: product.imageType,
                        isModalOpenEdit: true,
                        minimumQuantity:product.minimumQuantity,
                        id: id,
                    });


                }
                else {
                    console.log(response.message);
                }
            })
    }

    handleColumnRender = (item, index, column) => {
        if (column.fieldName === 'actions') {
            return <><PrimaryButton onClick={() => { this.handleEdit(item.id) }} style={{ marginRight: 10 }}>Edit</PrimaryButton>
                <DefaultButton onClick={() => { this.handleArchive(item.id); }}>Archive</DefaultButton></>
        }
        if (column.fieldName === 'quantity') {
            console.log(item)
            if (item.isLowStock) {
                return <>{item.quantity}
                    <TooltipHost content="Item has low stock!" id={item.id} calloutProps={{ gapSpace: 0 }}>

                        <FontIcon aria-describedby={item.id} iconName="WarningSolid" style={{
                            color: 'red', marginLeft: 5
                            , fontSize: 25, height: 25, width: 25
                        }} />
                    </TooltipHost>

                </>
            }
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

    handleOnSearch = async (newvalue) => {
        if (newvalue === '') {
            this.getAllItems();
            return;
        }
        await axios.get(process.env.REACT_APP_API_URL + `Product/Search?search=${newvalue}&page=${this.state.currentpage ?? 1}`, this.options)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
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

    getAllItems = async () => {
        await axios.get(process.env.REACT_APP_API_URL + `Product/GetAll?page=${this.state.currentpage ?? 1}`, this.options)
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

    handleArchive = (id) => {

        this.setState({ isYesNoHidden: false, archivedId: id })
        // this.setState({ allitems: this.state.allitems.filter((item) => item.id !== id) })
    }
    handleAnswer = async (e) => {
        this.setState({ isYesNoHidden: true })
        if (e === 'Yes') {
            this.setState({ allitems: this.state.allitems.filter((item) => item.id !== this.state.archivedId) })
            console.log(this.state.archivedId);
            var request = {
                productId: this.state.archivedId
            }
            await axios.put(process.env.REACT_APP_API_URL + `Product/Archive`, request, this.options)
                .then(res => {
                    var response = res.data;
                    if (response.succeeded) {
                        alert(response.message);
                    }
                    else {
                        console.log(response.message);
                    }
                });
        }
    }
    handleSaveEdit = async () => {
        const request = {
            productName: this.state.productName,
            productId: this.state.productId,
            categoryId: this.state.category,
            uOMId: this.state.UOM,
            quantity: this.state.quantity,
            minimumQuantity: this.state.minimumQuantity,
            price: this.state.price,
            productImage: this.state.selectedFile,
            id: this.state.id,
        }


        await axios.put(process.env.REACT_APP_API_URL + `Product/Update`, request, this.options)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    alert(response.message);
                    this.setState({ isModalOpen: false })
                }
                else {
                    console.log(response.message);
                }
            })
    }
    handleFileUpload = (e) => {
      
        const reader = new FileReader();
        var file = e.target.files[0];
        reader.onload = (event) => {
            this.setState({ selectedFile: event.target.result });
            console.log(event.target.result);

        }

        reader.readAsDataURL(file);
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
        return (

            <div className='mainbg'>
                <div style={{ height: '100%' }} className="sidebar">
                    <Navigation selectedNav='key3' />
                </div>
                <div className="frame" >

                    <PageHeader title='Inventory list' />
                    <div className="product-menu">
                        <div style={{ display: 'flex' }}>
                            <div className="divsearch">
                                <SearchBox placeholder="Search using product name or product id"
                                    onSearch={this.handleOnSearch}
                                    onClear={this.getAllItems} />
                                    
                            </div>
                            <div className="divbutton">
                            <DefaultButton onClick={() => window.location.href = '/categories'}>Categories</DefaultButton>
                            <DefaultButton onClick={() => window.location.href = '/uom'}>UOM</DefaultButton>
                                    
                            </div>
                            <YesNo message='Do you want to continue archiving?' hideModal={(this.state.isYesNoHidden)} onAnswer={this.handleAnswer} />
                            <div className="divbutton">
                                
                                <PrimaryButton onClick={() => this.setState({ isModalOpen: true })} style={{ marginRight: 10 }}>Add new</PrimaryButton>

                                <DefaultButton onClick={() => window.location.href = '/print'}> Print</DefaultButton>
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
                    <Pagination pagecount={this.state.pagecount} currentpage={this.state.currentpage} page='inventory' />
                    
                    <Modal
                        titleAriaId={1}
                        isOpen={this.state.isModalOpen}
                        onDismiss={() => this.setState({ isModalOpen: false })}
                        className="modal-container"
                        isBlocking={false}
                    >
                        <div className='modal-header'>
                            <h2 id={1}>
                                Add new product
                            </h2>
                            <IconButton
                                styles={iconButtonStyles}
                                iconProps={cancelIcon}
                                ariaLabel="Close popup modal"
                                onClick={() => { this.setState({ isModalOpen: false }) }}
                            />
                        </div>
                        <div className='modal-body'>
                            <Label>Image</Label>
                            <input type="file" accept=".png,.jpeg,.jpg" onChange={this.handleFileUpload} />
                            <TextField label="Product Name"
                                placeholder="Product name"
                                onChange={(e) => this.setState({ productName: e.target.value })} />

                            <TextField label="Product Id"
                                placeholder="Product Id"
                                onChange={(e) => this.setState({ productId: e.target.value })} />

                            <ComboBox label="Category"
                                autofill={false}
                                options={this.state.categories}
                                placeholder="Item Category"
                                onItemClick={(o, i, val) => { this.setState({ category: i.key }) }}
                                onChange={(o, i, val) => { this.setState({ category: i.key }) }} />

                            <ComboBox label="UOM"
                                autofill={false}
                                options={this.state.uomlist}
                                placeholder="UOM"
                                onItemClick={(o, i, val) => { this.setState({ UOM: i.key }) }}
                                onChange={(o, i, val) => { this.setState({ UOM: i.key }) }} />

                            <TextField label="Item Quantity"
                                placeholder="Quantity"
                                pattern="[0-9]*"
                                type="number"
                                onChange={(e) => this.setState({ quantity: e.target.value })} />
                            <TextField label="Minimum Quantity"
                                placeholder="Quantity"
                                pattern="[0-9]*"
                                type="number"
                                onChange={(e) => this.setState({ minimumQuantity: e.target.value })} />
                            <TextField label="Price"
                                placeholder="Price"
                                pattern="[0-9]*"
                                type="number"
                                onChange={(e) => this.setState({ price: e.target.value })} />
                        </div>

                        <div className='modal-header'>
                            <PrimaryButton onClick={this.handleAddNewProduct}>Submit</PrimaryButton>
                        </div>
                    </Modal>

                    <Modal
                        titleAriaId={2}
                        isOpen={this.state.isModalOpenEdit}
                        onDismiss={() => this.setState({ isModalOpenEdit: false })}
                        className="modal-container"
                        isBlocking={false}
                    >
                        <div className='modal-header'>
                            <h2 id={1}>
                                Edit product
                            </h2>
                            <IconButton
                                styles={iconButtonStyles}
                                iconProps={cancelIcon}
                                ariaLabel="Close popup modal"
                                onClick={() => { this.setState({ isModalOpenEdit: false }) }}
                            />
                        </div>

                        <div className='modal-body'>
                            <Label>Image</Label>
                            <img style={{ height: 50, width: 50 }} src={`${this.state.imagetype},${this.state.imageData}`} />
                            <br />
                            <input type="file" accept=".png,.jpeg,.jpg" onChange={this.handleFileUpload} text='Change image' />
                            <TextField label="Product Name"
                                placeholder="Product name"
                                onChange={(e) => this.setState({ productName: e.target.value })}
                                value={this.state.productName} />

                            <TextField label="Product Id"
                                placeholder="Product Id"
                                value={this.state.productId}
                                onChange={(e) => this.setState({ productId: e.target.value })} />

                            <ComboBox label="Category"
                                autofill={false}
                                options={this.state.categories}
                                placeholder="Item Category"
                                selectedKey={this.state.category}
                                onItemClick={(o, i, val) => { this.setState({ category: i.key }) }}
                                onChange={(o, i, val) => { this.setState({ category: i.key }) }} />

                            <ComboBox label="UOM"
                                autofill={false}
                                options={this.state.uomlist}
                                placeholder="UOM"
                                selectedKey={this.state.UOM}
                                onItemClick={(o, i, val) => { this.setState({ UOM: i.key }) }}
                                onChange={(o, i, val) => { this.setState({ UOM: i.key }) }} />

                            <TextField label="Item Quantity"
                                placeholder="Quantity"
                                pattern="[0-9]*"
                                type="number"
                                value={this.state.quantity}
                                onChange={(e) => this.setState({ quantity: e.target.value })} />

                            <TextField label="Minimum Quantity"
                                placeholder="Quantity"
                                pattern="[0-9]*"
                                type="number"
                                value={this.state.minimumQuantity}
                                onChange={(e) => this.setState({ minimumQuantity: e.target.value })} />

                            <TextField label="Price"
                                placeholder="Price"
                                pattern="[0-9]*"
                                type="number"
                                value={this.state.price}
                                onChange={(e) => this.setState({ price: e.target.value })} />
                        </div>

                        <div className='modal-header'>
                            <PrimaryButton onClick={this.handleSaveEdit}>Submit</PrimaryButton>
                        </div>
                    </Modal>
                </div>

            </div>

        )
    }
}

const cancelIcon: IIconProps = { iconName: 'Cancel' };
const theme = getTheme();
const iconButtonStyles: Partial<IButtonStyles> = {
    root: {
        color: theme.palette.neutralPrimary,
        marginLeft: 'auto',
        marginTop: '4px',
        marginRight: '2px',
    },
    rootHovered: {
        color: theme.palette.neutralDark,
    },
};