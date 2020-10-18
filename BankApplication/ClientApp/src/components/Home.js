import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'
import { TableRow } from './TableRow';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = { transactions: [], loading: true };
  }

  componentDidMount() {
    this.populateTransactionsData();
  }

  static renderTransactionsTable(transactions) {
    return (
      transactions.map(transaction =>
        <TableRow transaction={transaction}/>
      )
    );
  }

  render() {
    let rows = (this.state.transactions.length)
      ? Home.renderTransactionsTable(this.state.transactions)
      : <tr><td colSpan='5'>Brak operacji</td></tr>;

    let contents = (this.state.loading)
      ? <tr><td colSpan='5'><p><em>Loading...</em></p></td></tr>
      : rows;

    return (
      <div>
        <table className="table">
          <thead>
            <tr>
              <th>
                Odbiorca / Nadawca / Tytuł
            </th>
              <th>
                Data
            </th>
              <th>
                Kwota
            </th>
            </tr>
          </thead>
          <tbody className="table-transactions-result">
            {contents}
          </tbody>
        </table>
      </div>
    );
  }

  async populateTransactionsData() {
    const token = await authService.getAccessToken();
    const response = await fetch('transactions', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ transactions: data, loading: false });
  }
}
