import React, { Component } from 'react';
import { Redirect, useParams } from 'react-router-dom';
import OrderStepper from './OrderStepper';
import axios from "axios";
import { Typography } from '@material-ui/core';

export class Summary extends Component {
  static displayName = Summary.name;

  constructor(props) {
    super(props);

    this.state = {
      payment: this.props.data.payment,
      id: this.props.match.params.id
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