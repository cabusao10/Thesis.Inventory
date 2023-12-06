import react, { Component } from "react";

export class Pagination extends Component {
    constructor(props) {
        super(props)
        this.state = {
            pagecount: parseInt(props.pagecount),
            currentpage: parseInt(props.currentpage),
            page: props.page

        }
    }

    render() {
        let pages = [];
        for (var i = 0; i < this.state.pagecount; i++) {
            pages.push(<li class={`page-item ${this.state.currentpage == i+1 ? 'disabled':''}`}><a class="page-link" href={`/${this.state.page}?${i + 1}`}>{i + 1}</a></li>)
        }
        return (
            <>
                <nav aria-label="Page navigation example">
                    <ul class="pagination">
                        {this.state.currentpage > 1 && <li class="page-item"><a class="page-link" href="#">Previous</a></li>}
                        {pages}
                        {this.state.currentpage < this.state.pagecount && <li class="page-item"><a class="page-link" href="#">Next</a></li>}
                        {/* <li class="page-item"><a class="page-link" href="#">Previous</a></li>
                        <li class="page-item"><a class="page-link" href="#">1</a></li>
                        <li class="page-item"><a class="page-link" href="#">2</a></li>
                        <li class="page-item"><a class="page-link" href="#">3</a></li>
                        <li class="page-item"><a class="page-link" href="#">Next</a></li> */}
                    </ul>
                </nav>
            </>)
    }
}