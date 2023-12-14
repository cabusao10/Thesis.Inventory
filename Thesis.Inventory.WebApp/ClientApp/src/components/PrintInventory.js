import React, { Component, useState } from "react";
import { Navigation } from "./Navigation";
import { PageHeader } from "./PageHeader";
import { ComboBox, DefaultButton, DetailsList, DetailsListLayoutMode, FontWeights, IconButton, Image, Label, Modal, PrimaryButton, SearchBox, SelectionMode, TextField, createTheme, getTheme, loadTheme, mergeStyles } from "@fluentui/react";
import { Pagination } from "./Pagination";
import axios from "axios";
import { Link } from "react-router-dom";
import { faL } from "@fortawesome/free-solid-svg-icons";
import YesNo from "./YesOrNoModal";
import { useReactToPrint } from 'react-to-print';

export class PrintInventory extends Component {
    constructor(props) {
        super(props);

        this.state = {
            allitems: [],
            categories: [],
            uomlist: [],
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

    columns = [{ key: 'column1', name: 'Id', fieldName: 'id', minWidth: 20, maxWidth: 20 },
    { key: 'column2', name: 'Product Name', fieldName: 'productName', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column3', name: 'Product Id', fieldName: 'productId', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column4', name: 'Category', fieldName: 'categoryId', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column5', name: 'UOM', fieldName: 'uomId', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column6', name: 'Price', fieldName: 'price', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column7', name: 'Quantity', fieldName: 'quantity', minWidth: 100, maxWidth: 200, isResizable: true },]

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
    targetRef = React.createRef();
    handleColumnRender = (item, index, column) => {

        if (column.fieldName === 'quantity') {
            return <>{item.quantity}</>
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

    render() {
        return (
            <div className='mainbg-print'>
                <div className="frame-print" > <PrintPDF target={this.targetRef} />
                    <br />
                    <div ref={this.targetRef} > <DetailsList

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

                    /></div></div>


            </div>

        )


    }
}

function PrintPDF(e) {
    const handlePrint = useReactToPrint({
        content: () => e.target.current,
        documentTitle: 'Inventory List',
        onAfterPrint: () => console.log('Printed PDF successfully!'),
    });

    return (<><PrimaryButton onClick={handlePrint}>Print</PrimaryButton></>)

}