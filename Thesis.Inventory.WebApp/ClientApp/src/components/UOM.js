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
export class UOM extends Component {

    constructor(props) {
        super(props)
        this.state = {

            pagecount: 0,
            currentpage: urlParams.get('page') ?? 1,
            isModalOpen: false,
            categories: [],
            categoryName: '',
            categeoryDetails: '',
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
        this.getAllCategories();
    }
    options = {
        headers: {
            'Content-type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
        }
    }

    columns = [{ key: 'column1', name: 'Id', fieldName: 'id', minWidth: 20, maxWidth: 20 },
    { key: 'column2', name: 'Name', fieldName: 'name', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column3', name: 'Description', fieldName: 'description', minWidth: 100, maxWidth: 200, isResizable: true },
    ]

    getAllCategories = async () => {
        await axios.get(process.env.REACT_APP_API_URL + `Product/GetUom`, this.options)
            .then(res => {
                var response = res.data;
                console.log(response)
                if (response.succeeded) {

                    this.setState({ categories: response.data })
                }
                else {
                    console.log(response.message);
                }
            })
    }

    handleAddCategory = async()=> {
        var request = {
            name: this.state.categoryName,
            description: this.state.categeoryDetails
        }

        await axios.post(process.env.REACT_APP_API_URL + `Product/AddUom`, request, this.options)
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
    render() {
        return (<div>
            <div className='mainbg'>
                <div style={{ height: '100%' }} className="sidebar">
                    <Navigation selectedNav='key3' />
                </div>
                <div className="frame" >

                    <PageHeader title='Unit of measurement' />
                    <div className="product-menu">
                        <div style={{ display: 'flex' }}>

                            <YesNo message='Do you want to continue archiving?' hideModal={(this.state.isYesNoHidden)} onAnswer={this.handleAnswer} />
                            <div className="divbutton">
                                <PrimaryButton onClick={() => this.setState({ isModalOpen: true })} style={{ marginRight: 10 }}>Add new</PrimaryButton>


                            </div>
                        </div>
                    </div>
                    <DetailsList
                        onRenderItemColumn={this.handleColumnRender}
                        items={this.state.categories}
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


                    <Modal
                        titleAriaId={1}
                        isOpen={this.state.isModalOpen}
                        onDismiss={() => this.setState({ isModalOpen: false })}
                        className="modal-container"
                        isBlocking={false}
                    >
                        <div className='modal-header'>
                            <h2 id={1}>
                                Add new UOM
                            </h2>
                            <IconButton
                                styles={iconButtonStyles}
                                iconProps={cancelIcon}
                                ariaLabel="Close popup modal"
                                onClick={() => { this.setState({ isModalOpen: false }) }}
                            />
                        </div>
                        <div className='modal-body'>

                            <TextField label="Name"
                                placeholder="UOM name"
                                onChange={(e) => this.setState({ categoryName: e.target.value })} />

                            <TextField label="Details"
                                placeholder="Decription"
                                onChange={(e) => this.setState({ categeoryDetails: e.target.value })} />


                        </div>

                        <div className='modal-header'>
                            <PrimaryButton onClick={this.handleAddCategory}>Submit</PrimaryButton>
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

                            <TextField label="Item Quantity"
                                placeholder="Quantity"
                                pattern="[0-9]*"
                                type="number"
                                value={this.state.quantity}
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
        </div>)
    }
}