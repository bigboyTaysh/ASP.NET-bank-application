import React, { Component, useEffect, useState } from 'react';
import { Redirect } from 'react-router-dom';
import OrderStepper from './OrderStepper';
import ProductList from './ProductList';
import axios from "axios";
import { CircularProgress, Grid, makeStyles, Paper, Typography } from '@material-ui/core';
import CheckCircleOutlineIcon from '@material-ui/icons/CheckCircleOutline';

const useStyles = makeStyles((theme) => ({
  root: {
    margin: theme.spacing(1),
    width: theme.spacing(16),
    height: theme.spacing(16),
  },
  paper: {
    marginTop: 20,
    padding: theme.spacing(4),
    textAlign: 'center',
    backgroundColor: "#3d3d3d",
  },
  text: {
    fontSize: theme.typography.pxToRem(25),
  },
  paperChild: {
    margin: "auto",
    width: "60%"
  },
  successIcon: {
    fontSize: theme.typography.pxToRem(70),
    color: "#4caf50"
  }
}));

export default function Summary(props) {
  const classes = useStyles();
  const [orderId, setOrderId] = useState(props.match.params.id || '');;
  const [payment, setPayment] = useState(props.data.payment || '');
  const [itemsCount, setItemsCount] = useState(props.data.itemsCount || '');
  const [basketPrice, setBasketPrice] = useState(props.data.basketPrice || '');
  const [basket, setBasket] = useState(props.data.basket || '');
  const [order, setOrder] = useState('');

  useEffect(() => {
    if (props.data.itemsCount != 0) {
      props.handleBasketReset()
    }
    if (!payment && !isNaN(orderId)) {

    }
  });

  let status = basket != '' ? (
    <Grid item xs={12} >
      <CheckCircleOutlineIcon className={classes.successIcon} />
      <Typography variant="h4">
        Zamówienie zostało przyjęte
      </Typography>
    </Grid>
  ) : (
      <Grid item xs={12}>
        <CircularProgress />
        <Typography variant="h4">
          Ładowanie..
      </Typography>
      </Grid>

    )

  return (
    (payment && orderId === 'cashOnDelivery') || (!payment && !isNaN(orderId)) ? (
      <div>
        <ProductList data={{ basket: basket }} />
        <Paper
          className={classes.paper}
        >
          <Paper className={classes.paperChild}>
            <Grid
              container
              direction="column"
              justify="center"
              alignItems="center"
              spacing={2}
            >
              {status}
            </Grid>
          </Paper>
        </Paper>
        <OrderStepper step={2} />
      </div>
    ) : (
        < Redirect to='/' />
      )
  );
}