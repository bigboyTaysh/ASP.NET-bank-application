import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'
import { BankAccountContainer } from './BankAccountContainer';
import { TransactionRow } from './TransactionRow';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {
                transactions: [],
                bankAccounts: [],
                index: 1,
                loading: true 
        };

        this.renderBankAccounts = this.renderBankAccounts.bind(this);
        this.renderTransactionsTable = this.renderTransactionsTable.bind(this);
        this.onChangeActiveBankAccount = this.onChangeActiveBankAccount.bind(this);
    }

    componentDidMount() {
        this.populateBankAccountsData();
    }

    onChangeActiveBankAccount(index){
        this.setState({
            index: index
        })
    }

    renderBankAccounts(bankAccounts, indexState) {
        return (
            bankAccounts.map((bankAccount, index) =>
                <BankAccountContainer key={bankAccount.id} bankAccount={bankAccount} clicked={index === indexState } index={index} onClick={this.onChangeActiveBankAccount} />
            )
        );
    }

    renderTransactionsTable(transactions) {
        return (
            transactions.map(transaction=>
                <TransactionRow key={transaction.id} transaction={transaction} />
            )
        );
    }

    render() {
        let bankAccounts = this.state.bankAccounts;

        let rows = (this.state.transactions.length)
            ? this.renderTransactionsTable(this.state.transactions)
            : <tr><td colSpan='5'>Brak operacji</td></tr>;

        let bankAccountsContents = (this.state.loading)
            ? <p><em>Loading...</em></p>
            : bankAccounts;

        let rowContents = (this.state.loading)
            ? <tr><td colSpan='5'><p><em>Loading...</em></p></td></tr>
            : rows;

        return (
            
            <div >
                <div className="jumbotron">
                {bankAccounts.map((bankAccount, index) =>
                    <BankAccountContainer 
                        key={bankAccount.id} 
                        bankAccount={bankAccount} 
                        clicked={index === this.state.index }
                        index={index} 
                        onClick={this.onChangeActiveBankAccount} 
                    />
                )}
                </div>
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
                        {rowContents}
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

    async populateBankAccountsData() {
        const token = await authService.getAccessToken();
        const response = await fetch('bankaccounts', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        const data = await response.json();
        this.setState({ bankAccounts: data, loading: false });
        //this.populateTransactionsData();
    }
}
