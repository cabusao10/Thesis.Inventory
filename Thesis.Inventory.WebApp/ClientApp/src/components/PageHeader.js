import react, { Component } from "react";

export class PageHeader extends Component {
    constructor(props) {
        super(props);
        this.state = { title: props.title };
    }

    render() {
        return (
            <div className="page-header">
                <h2>{this.state.title}</h2>
            </div>
        )
    }
}