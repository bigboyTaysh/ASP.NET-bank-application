import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import OrderStepper from './OrderStepper';

export class Summary extends Component {
  static displayName = Summary.name;

  constructor(props) {
    super(props);

    this.state = {
      payment: this.props.data.payment
    }
  }

  componentDidMount() {
    this.props.handleBasketReset();
  }

  render() {
    return (
      this.state.payment ?
        (<div>
          <OrderStepper step={2} />
        </div>) : (
          <Redirect to='/' />
        )
    );
  }
}