import { Button, FormControl, FormHelperText, Grid, InputLabel, makeStyles, MenuItem, Paper, Select, TextField, Typography } from '@material-ui/core';
import React from 'react';
import NumberFormat from 'react-number-format';
import { useHistory } from 'react-router-dom';
import OrderStepper from './OrderStepper';
import PropTypes from 'prop-types';
import axios from 'axios';
import { Alert } from '@material-ui/lab';


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
    width: '23ch'
  },
  pin: {
    width: '10ch'
  },
  alert: {
    width: '40ch'
  },
}));

export default function PaymentForm(props) {
  const classes = useStyles();
  const [cardPayment, setCardPayment] = React.useState('');
  const [status, setStatus] = React.useState('');
  const [values, setValues] = React.useState({
    address: '',
    cardNumber: '',
    code: ''
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

  const addOrder = () => {
    axios.post('api/orders', {
      price: props.data.basketPrice,
      items: props.data.basket.map(product => ({
        product: product
      }))
    })
      .then(function (response) {
        window.location =
         'https://localhost:44377/paymentCards/cardPayment?orderId=' + response.data.id
         + '&apiKey=' + '2a9f86fc-8fd6-439d-99af-30d743180d6a'
         + '&cardNumber=' + values.cardNumber;
      })
      .catch(function (error) {
        setStatus("error");
      })
}

const handleSumbit = () => {
  axios.post('https://localhost:44339/api/payment/cardSecure', {
    apiKey: "ad777c2b-d332-4107-838a-b37738fa8e1f",
    cardNumber: values.cardNumber,
    code: values.code
  })
    .then(function (response) {
      if (response.data.status) {
        addOrder();
      } else {
        setStatus(response.data.status);
      }
    })
    .catch(function (error) {
      if (error.response) {
        setStatus(error.response.status);
      } else {
        setStatus("error");
      }
    })

  //props.handleSetPayment(true);
}

let button = values.address.length > 5 && values.cardNumber.trim().length == 19 && values.code.trim().length == 4 ? (
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

let statusText;

let cardSecured =
  <Typography>
    Podana karta jest zabezpieczona
    </Typography>

let cardNotSecured =
  <Alert className={classes.alert} severity="warning">Karta nie widnieje w systemie CardSecure</Alert>

let cardNotFound =
  <Alert className={classes.alert} severity="error">Nieprawidłowe dane karty</Alert>

let wrong =
  <Alert className={classes.alert} severity="error">Coś poszło nie tak</Alert>

if (status === true) {
  statusText = (cardSecured)
} else if (status === false) {
  statusText = (cardNotSecured)
} else if (status === 404) {
  statusText = (cardNotFound)
} else if (status === '') {
  statusText = ''
} else {
  statusText = (wrong)
}

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
          alignItems="center"
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
          <Grid item>
            {statusText}
          </Grid>
          <form className={classes.form} noValidate autoComplete="off">
            <Grid item>
              <TextField
                required
                label="Adres"
                name="address"
                onChange={handleInputChange}
                id="address"
                variant="outlined"
              />
            </Grid>
            <Grid item xs={12}>
              <NumberFormat
                required
                label="Numer karty płatniczej"
                onChange={handleInputChange}
                name="cardNumber"
                id="cardNumber"
                variant="outlined"
                customInput={TextField}
                className={classes.bankAccountNumber}
                isNumericString
                format="#### #### #### ####"
              />
            </Grid>
            <Grid item xs={12}>
              <NumberFormat
                required
                label="Kod"
                onChange={handleInputChange}
                name="code"
                id="code"
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