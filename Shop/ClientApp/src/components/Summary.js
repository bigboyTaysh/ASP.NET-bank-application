import React, { Component, useEffect, useState } from 'react';
import { Redirect } from 'react-router-dom';
import OrderStepper from './OrderStepper';
import ProductList from './ProductList';
import axios from "axios";
import { Grid, makeStyles, Paper, Typography } from '@material-ui/core';

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
  }
}));

export default function Summary(props) {
  const classes = useStyles();
  const [payment, setPayment] = useState(props.data.payment || '')
  const [itemsCount, setItemsCount] = useState(props.data.itemsCount || '')
  const [basketPrice, setBasketPrice] = useState(props.data.basketPrice || '')
  const [basket, setBasket] = useState(props.data.basket || '')

  let content;

  useEffect(() => {
    if (props.data.itemsCount != 0) {
      props.handleBasketReset()
    }
  });

  if (payment && props.match.params.id === 'cashOnDelivery') {
    content = <div>
      <ProductList data={{basket: basket}}/>
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
            <Grid item xs={12} >
              <Paper>
                <Typography className={classes.text}>
                  Koszt zamówienia
              </Typography>
              </Paper>
            </Grid>
            <Grid item xs={8}>
              <Paper>
                <Typography className={classes.text}>
                  {basketPrice} zł
              </Typography>
              </Paper>
            </Grid>
          </Grid>
        </Paper>
      </Paper>
      <OrderStepper step={2} />
    </div>
  } else {
    content = <Redirect to='/' />
  }

  return (
    content
  );
}