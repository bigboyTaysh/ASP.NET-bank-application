import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import OrderStepper from './OrderStepper';
import axios from "axios";
import { Grid, Paper, Typography } from '@material-ui/core';

export class Summary extends Component {
  static displayName = Summary.name;

  constructor(props) {
    super(props);

    this.state = {
      payment: this.props.data.payment,
      id: this.props.match.params.id,
      basketPrice: this.props.data.basketPrice
    }
  }

  componentDidMount() {
    this.props.handleBasketReset();
  }

  render() {
    return (
      this.state.payment ?
        (<div>
          <Paper>
            <Grid
              container
            >
              <Grid
                item
              >
                <Typography>
                  {this.state.basketPrice}
                </Typography>
              </Grid>
            </Grid>
          </Paper>
          <OrderStepper step={2} />
        </div>) : (
          <Redirect to='/' />
        )
    );
  }
}