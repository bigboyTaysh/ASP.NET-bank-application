import React, { Component } from 'react';

export class TableRow extends Component {
    static displayName = TableRow.name;

    constructor(props) {
        super(props);
        this.state = {
            clicked: false
        }
    }

    render () {
        return (
            <tr key={this.props.transaction.id}>
                <td>{this.props.transaction.date}</td>
                <td>{this.props.transaction.description}</td>
            </tr>
        )
    }
}
