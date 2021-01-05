import { Button, FormControl, FormHelperText, Grid, InputLabel, makeStyles, MenuItem, Paper, Select, TextField, Typography } from '@material-ui/core';
import React from 'react';
import NumberFormat from 'react-number-format';
import { useHistory } from 'react-router-dom';
import OrderStepper from './OrderStepper';
import PropTypes from 'prop-types';


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
    width: "80%"
  },
  form: {
    '& .MuiTextField-root': {
      margin: theme.spacing(1),
    },
  },
  bankAccountNumber: {
    width: '35ch'
  }, 
  pin: {
    width: '10ch'
  }
}));

export default function PaymentForm(props) {
  const classes = useStyles();
  const [cardPayment, setCardPayment] = React.useState('');
  const [values, setValues] = React.useState({
    address: '',
    cardNumber: '',
    cardPin: ''
  });

  let history = useHistory();

  const handleChange = (event) => {
    setCardPayment(event.target.value);
  };

  const handleInputChange = (event) => {
    setValues({
      ...values,
      [event.target.name]: event.target.value,
    });
  };

  const handleSumbit = () => {
    if (!cardPayment) {
      //props.handleSetPayment(true);
      //history.push('/summary/cashOnDelivery')
    }
  }

  var button = props.data.itemsCount > 0 && cardPayment !== '' ? (
    <Button
      variant="contained"
      color="primary"
      onClick={handleSumbit}
    >
      Zapłać
    </Button>
  ) : (
      <Button
        variant="contained"
        color="primary"
        disabled
      >
        Zapłać
      </Button>
    )

  return (
    <div>
      <Paper
        className={classes.paper}
      >
        <Paper className={classes.paperChild}>
          <Grid
            container
            direction="column"
            justify="center"
            alignItems="stretch"
            spacing={2}
          >
            <Grid item>
              <Paper>
                <Typography className={classes.text}>
                  Koszt zamówienia
              </Typography>
              </Paper>
            </Grid>
            <Grid item>
              <Typography className={classes.text}>
                {props.data.basketPrice} zł
                </Typography>
            </Grid>
            <form className={classes.form} noValidate autoComplete="off">
              <Grid item>
                <TextField
                  required
                  label="Adres"
                  onChange={handleInputChange}
                  id="address"
                  variant="outlined"
                />
              </Grid>
              <Grid item xs={12}>
                <NumberFormat
                  required
                  label="Numer konta bankowego"
                  onChange={handleInputChange}
                  name="cardNumber"
                  id="cardNumber"
                  variant="outlined"
                  customInput={TextField}
                  className={classes.bankAccountNumber}
                  isNumericString
                  format="## #### #### #### #### #### ####"
                />
              </Grid>
              <Grid item xs={12}>
                <NumberFormat
                  required
                  label="Kod"
                  onChange={handleInputChange}
                  name="cardPin"
                  id="cardPin"
                  variant="outlined"
                  customInput={TextField}
                  className={classes.pin}
                  isNumericString
                  format="####"
                />
              </Grid>
            </form>
            <Grid item>
              {button}
            </Grid>
          </Grid>
        </Paper>
      </Paper>
      <OrderStepper step={1} />
    </div>
  );
}