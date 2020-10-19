import React, { Component } from 'react';

export class BankAccountContainer extends Component {
    static displayName = BankAccountContainer.name;

    constructor(props) {
        super(props);
        this.state = {
            index: props.index
        }
        this.changeActiveState = this.changeActiveState.bind(this);
    }

    changeActiveState(){
        if(!this.props.clicked){
            this.props.onClick(this.state.index);
        }
    };

    render() {
        let bankAccountName;
        if (this.props.bankAccount.type === "PAY_ACC_FOR_YOUNG") {
            bankAccountName = "Konto dla młodych";
        } else if (this.props.bankAccount.type === "PAY_ACC_FOR_ADULT") {
            bankAccountName = "Superkonto"
        } else {
            bankAccountName = "Konto walutowe"
        }

        let content =
            <div>
                <div className="details-type">
                    <div>
                        {bankAccountName}
                    </div>
                    <div className="bankAccountSelect gray-text">{this.props.bankAccount.bankAccountNumber}</div>
                </div>
                <div className="pull-right noselect">
                    <div className="pull-right text-align-right details-menas">
                        <span>Dostępne środki</span>
                        <br />
                        <span>
                            <b>
                                {this.props.bankAccount.availableFounds.toFixed(2)}
                                {this.props.bankAccount.currency.code}
                            </b>
                        </span>
                    </div>
                    <div className="pull-right noselect">
                        <div className="pull-right text-align-right details">
                            <span className="gray-text">
                                {this.props.bankAccount.balance.toFixed(2)}
                                {this.props.bankAccount.currency.code}
                            </span>
                            <br />
                            <span className="gray-text">
                                {this.props.bankAccount.lock.toFixed(2)}
                                {this.props.bankAccount.currency.code}
                            </span>
                        </div>
                        <div className="pull-right text-align-left details">
                            <span className="gray-text">
                                Sadlo
                        </span>
                            <br />
                            <span className="gray-text">
                                Blokady
                        </span>
                        </div>
                    </div>
                </div>
                <div className="noselect">
                    <br />
                    <br />
                </div>
                <div className="pull-right buttons noselect">


                    {/*
                    @Html.ActionLink("Szczegóły", "Details", "BankAccounts", new {id = Model[0].BankAccountTypeID}, new { @class = "btn btn-default" })
                    @Html.ActionLink("Historia", "Index", "Transactions", new {bankAccountNumber = Model[0].BankAccountNumber}, new { @class = "btn btn-default" })
                    @Html.ActionLink("Przelew", "Transfer", "Transactions", null, new { @class = "btn btn-primary" })
                    */}
                </div>
            </div>;

        return (
            <div onClick={this.changeActiveState} className={(this.props.clicked ? "active " : "") + "bank-account-div shadow p-3 mb-5 bg-white rounded"}>
                {content}
            </div>
        )
    }
}