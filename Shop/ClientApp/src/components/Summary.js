import React, { Component, useEffect, useState } from 'react';
import { Redirect, useHistory } from 'react-router-dom';
import OrderStepper from './OrderStepper';
import ProductList from './ProductList';
import axios from "axios";
import { Button, CircularProgress, Grid, makeStyles, Paper, Typography } from '@material-ui/core';
import { CheckCircleOutline, RemoveCircleOutline, Timer } from '@material-ui/icons';
import Moment from 'react-moment';


const useStyles = makeStyles((theme) => ({
  paper: {
    marginTop: 20,
    padding: theme.spacing(5),
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
    color: "green"
  },
  failureIcon: {
    fontSize: theme.typography.pxToRem(70),
    color: "red"
  },
  waitIcon: {
    fontSize: theme.typography.pxToRem(70),
    color: "orange"
  },
  grid: {
    margin: theme.spacing(5),
  },
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

  const handleSumbit = () => {
    window.location =
      'https://localhost:44377/paymentCards/cardPayment?orderId=' + order.id
      + '&apiKey=' + '2a9f86fc-8fd6-439d-99af-30d743180d6a'
      + '&cardNumber=' + order.cardNumber;
  }

  if (payment && orderId === 'cashOnDelivery') {
    content =
      <Grid item xs={12} >
        <CheckCircleOutline className={classes.successIcon} />
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
    let icon, status, pay, date;

    if (order.status.status === 'Completed') {
      icon = <CheckCircleOutline className={classes.successIcon} />;
      status = 'Zamówienie zostało przyjęte';
      date = 'Data przyjęcia zamówienia: '
    } else if (order.status.status === 'Declined') {
      icon = <RemoveCircleOutline className={classes.failureIcon} />;
      status = 'Płatność odrzucona';
      date = 'Data anulowania zamówienia: ';
    } else if (order.status.status === 'Awaiting Payment') {
      icon = <Timer className={classes.waitIcon} />;
      status = 'Oczekiwanie na płatność';
      date = 'Data złożenia: ';
      pay = <Grid item xs={12} className={classes.grid}>
        <Button
          variant="contained"
          color="primary"
          onClick={handleSumbit}
        >
          Zapłać
      </Button>
      </Grid>
    }

    content = <div>
      <Grid item xs={12} className={classes.grid}>
        {icon}
        <Typography variant="h4">
          {status}
        </Typography>
      </Grid>
      <Typography variant="subtitle1">
        {date}
      </Typography>
      <Typography variant="subtitle1">
        <Moment format="HH:mm DD-MM-YYYY">
          {order.date}
        </Moment>
      </Typography>
      <Grid item xs={12} className={classes.grid}>
        <Typography variant="h5">
          Cena całkowita
          </Typography>
        <Typography variant="h5">
          {order.price} zł
        </Typography>
      </Grid>
      {pay}
    </div>
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