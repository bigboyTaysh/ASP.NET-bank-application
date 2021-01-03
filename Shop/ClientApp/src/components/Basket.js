import { Button, Grid, makeStyles, Paper, Typography } from '@material-ui/core';
import React, { Component } from 'react';
import ProductList from './ProductList';


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
    width: "50%"
  }
}));

export default function Basket(props) {
  const classes = useStyles();

  return (
    <div>
      <ProductList data={props.data} handleProductRemoveClick={props.handleProductRemoveClick} />
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
            <Grid item xs={6} >
              <Paper>
                <Typography className={classes.text}>
                  Koszt zamówienia
              </Typography>
              </Paper>
            </Grid>
            <Grid item xs={6}>
              <Paper>
                <Typography className={classes.text}>
                  {props.data.basketPrice} zł
              </Typography>
              </Paper>
            </Grid>
            <Grid item xs={6}>
              <Button
                variant="contained"
                color="primary"
              >
                Złóż zamówienie
              </Button>
            </Grid>
          </Grid>
        </Paper>
      </Paper>
    </div>
  );
}