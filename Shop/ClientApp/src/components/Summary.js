import React, { Component, useEffect, useState } from 'react';
import { Redirect, useHistory } from 'react-router-dom';
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
  const history = useHistory();
  const [orderId, setOrderId] = useState(props.match.params.id);;
  const [payment, setPayment] = useState(props.data.payment);
  const [itemsCount, setItemsCount] = useState(props.data.itemsCount);
  const [basketPrice, setBasketPrice] = useState(props.data.basketPrice);
  const [basket, setBasket] = useState(props.data.basket);
  const [order, setOrder] = useState();
  const [contnet, setContnet] = useState();
  const [error, setError] = useState();

  let content, list;

  useEffect(() => {
    if (props.data.itemsCount != 0) {
      props.handleBasketReset()
    }
    if (!order && !error) {
      axios.get('api/orders/' + orderId)
        .then(function (response) {
          console.log(response.data)
          setOrder(response.data)
        })
        .catch(function (error) {
          setError(error);
        })
    }
  }, []);
  
  const redirect = () => {
    history.push('/')
  }

  if (payment && orderId === 'cashOnDelivery') {
    content =
      <Grid item xs={12} >
        <CheckCircleOutlineIcon className={classes.successIcon} />
        <Typography variant="h4">
          Zamówienie zostało przyjęte
        </Typography>
      </Grid>
  } else if (error) {
    redirect();
  } else if (!payment && !isNaN(orderId) && !order) {
    content =
      <Grid item xs={12}>
        <CircularProgress />
        <Typography variant="h4">
          Ładowanie..
        </Typography>
      </Grid>
  } else if (order) {
    content =
      <Grid item xs={12}>
        <Typography variant="h4">
          Załadowano
        </Typography>
      </Grid>
  }

  if (payment && basket) {
    list = <ProductList data={{ basket: basket }} />
  } else if (order) {
    list = <ProductList data={{ basket: order.items.flatMap(i => i.product) }} />
  }

  return (
    (payment && orderId === 'cashOnDelivery') || (!payment && !isNaN(orderId)) ? (
      <div>
        {list}
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
              {content}
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