import { ComboBox, DefaultButton, DetailsList, DetailsListLayoutMode, IconButton, Modal, Label, PrimaryButton, SelectionMode, TextField, createTheme, getTheme, loadTheme, DatePicker } from "@fluentui/react";
import react, { Component } from "react";
import { Navigation } from "./Navigation";
import YesNo from "./YesOrNoModal";
import { PageHeader } from "./PageHeader";
import axios from "axios";



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
const Gender = {
    0: 'Male',
    1: 'Female',
    2: 'Undefined',
}
const UserRole = {
    0: 'Admin',
    1: 'Staff',
    2: 'Consumer',
    3: 'Supplier',
}

const UserRoleoption :IComboBoxOption[] = [
    {key:'0',text:'Admin'},
    {key:'1',text:'Staff'},
    {key:'2',text:'Consumer'},
    {key:'3',text:'Supplier'},
]
const UserStatus = {
    0: 'Not Verified',
    1: 'Verified',
}

export default class Users extends Component {

    constructor(props) {
        super(props)
        this.state = {
            allitems: [],
            currentpage: 1,
            pagecount: 0,
            isModalOpen: false,
            id: 0,
            provinces: [],
            fullname: '',
            businessName: '',
            username: '',
            provinceId: '3',
            address: '',
            zipCode: '',
            email: '',
            phoneNumber: '',
            gender: 0,
            birthdate: ''
        }

        this.getAllUsers();
        this.fetchProvince();
    }

    getAllUsers = async () => {
        await axios.get(process.env.REACT_APP_API_URL + `User/GetAll`)
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
            })
    }

    handleEditUser = async (id) => {
        this.setState({ isModalOpen: true, archivedId: id })

        await axios.get(process.env.REACT_APP_API_URL + `User/GetUserById?id=${id}`)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    console.log(response.data);
                    this.setState(response.data)
                    console.log(this.state)
                }
                else {
                    console.log(response.message);
                }
            })
    }

    handleColumnRender = (item, index, column) => {
        if (column.fieldName === 'actions') {
            return <><PrimaryButton onClick={() => this.handleEditUser(item.id)} style={{ marginRight: 10 }}>Edit</PrimaryButton>
            </>
        }
        else if (column.fieldName === 'status') {
            var status = UserStatus[item.status]
            return <>{status}</>
        }
        else if (column.fieldName === 'role') {
            var role = UserRole[item.role]
            return <>{role}</>
        }

        else if (column.fieldName === 'gender') {
            var gender = Gender[item.gender]
            return <>{gender}</>
        }
        return item[column.fieldName];
    }

    columns = [{ key: 'column1', name: 'Id', fieldName: 'id', minWidth: 20, maxWidth: 20 },
    { key: 'column2', name: 'Username', fieldName: 'username', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column3', name: 'Email', fieldName: 'email', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column4', name: 'Contact Number', fieldName: 'contactNumber', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column5', name: 'Address', fieldName: 'address', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column6', name: 'Zip Code', fieldName: 'zipCode', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column7', name: 'Province', fieldName: 'provinceId', minWidth: 100, maxWidth: 200, isResizable: true },
    { key: 'column8', name: 'Birthday', fieldName: 'birthdate', minWidth: 200, maxWidth: 200, isResizable: true },
    { key: 'column9', name: 'Gender', fieldName: 'gender', minWidth: 200, maxWidth: 200, isResizable: true },
    { key: 'column10', name: 'Role', fieldName: 'role', minWidth: 200, maxWidth: 200, isResizable: true },
    { key: 'column11', name: 'Status', fieldName: 'status', minWidth: 200, maxWidth: 200, isResizable: true },
    { key: 'column12', name: '', fieldName: 'actions', minWidth: 200, maxWidth: 200, isResizable: true },
    ]

    fetchProvince = () => {
        axios.get(process.env.REACT_APP_API_URL + "User/GetProvinces")
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    this.setState({ provinces: response.data })
                }
                else {
                    alert(response.message);
                }

            });
    }

    handleUpdate = async () => {
        var request = {
            id: this.state.id,
            businessName: this.state.businessName,
            fullname: this.state.fullname,
            contactNumber: this.state.contactNumber,
            address: this.state.address,
            zipCode: this.state.zipCode,
            provinceId: this.state.provinceId,
            birthdate: this.state.birthdate,
            gender: this.state.gender,
            role: this.state.role
        }

        console.log(request);
        axios.patch(process.env.REACT_APP_API_URL + "User/UpdateUser", request)
            .then(res => {
                var response = res.data;
                if (response.succeeded) {
                    alert("Success updating the user.")
                }
                else {

                }
            })
    }
    render() {
        loadTheme(myTheme);

        return (
            <div className='mainbg'>
                <div style={{ height: '100%' }} className="sidebar">
                    <Navigation selectedNav='key7' />
                </div>
                <div className="frame" >

                    <PageHeader title='User list' />
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
                <Modal
                    titleAriaId={1}
                    isOpen={this.state.isModalOpen}
                    onDismiss={() => this.setState({ isModalOpen: false })}
                    className="modal-container"
                    isBlocking={false}
                >
                    <div className='modal-header'>
                        <h2 id={1}>
                            Edit user
                        </h2>
                        <IconButton
                            styles={iconButtonStyles}
                            iconProps={cancelIcon}
                            ariaLabel="Close popup modal"
                            onClick={() => { this.setState({ isModalOpen: false }) }}
                        />
                    </div>
                    <div className='modal-body'>
                        <div class="input-box">
                            <TextField label='Full name' required placeholder='Enter your full name'
                                value={this.state.fullname}
                                onChange={e => { this.setState({ fullname: e.target.value }) }} />
                        </div>
                        <div class="input-box">
                            <TextField label='Business Name' required placeholder='Enter your business name'
                                value={this.state.businessName}
                                onChange={e => { this.setState({ businessName: e.target.value }) }} />

                        </div>
                        <div class="input-box">
                            <ComboBox label='Province'
                                options={this.state.provinces} required
                                onItemClick={(o, i, val) => { this.setState({ provinceId: i.key }) }}
                                selectedKey={`${this.state.provinceId}`}
                                onChange={(o, i, val) => { this.setState({ provinceId: i.key }) }} />
                        </div>
                        <div class="input-box">
                            <TextField label='Address' required placeholder='Enter your address'
                                value={this.state.address}
                                onChange={e => { this.setState({ address: e.target.value }) }} />
                        </div>
                        <div class="input-box">
                            <TextField label='Zip Code' required placeholder='Enter your Zip Code'
                                value={this.state.zipCode}
                                onChange={e => { this.setState({ zipCode: e.target.value }) }} />
                        </div>
                        <div class="input-box">
                            <TextField label='Email' type='email' required placeholder='Enter your email'
                                value={this.state.email}
                                onChange={e => { this.setState({ email: e.target.value }) }} />
                        </div>
                        <div class="input-box">
                            <TextField label='Phone Number' required placeholder='Enter your phone number'
                                value={this.state.contactNumber}
                                onChange={e => { this.setState({ contactNumber: e.target.value }) }} />
                        </div>
                        <div class="input-box">
                            <ComboBox label='Role'
                                options={UserRoleoption} required
                                onItemClick={(o, i, val) => { this.setState({ role: i.key }) }}
                                selectedKey={`${this.state.role}`}
                                onChange={(o, i, val) => { this.setState({ role: i.key }) }} />
                        </div>
                    </div>

                    <div className='modal-header'>
                        <PrimaryButton onClick={this.handleUpdate}>Submit</PrimaryButton>
                    </div>
                </Modal>

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