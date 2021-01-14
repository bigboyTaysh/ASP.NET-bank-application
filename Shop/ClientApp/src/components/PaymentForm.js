import { Backdrop, Button, CircularProgress, Grid, makeStyles, Paper, TextField, Typography } from '@material-ui/core';
import React, { useEffect } from 'react';
import NumberFormat from 'react-number-format';
import OrderStepper from './OrderStepper';
import axios from 'axios';
import { Alert } from '@material-ui/lab';
import authService from './api-authorization/AuthorizeService';
import { useHistory } from 'react-router-dom';


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
  backdrop: {
    zIndex: theme.zIndex.drawer + 1,
    color: '#fff',
  },
}));

export default function PaymentForm(props) {
  const classes = useStyles();
  let history = useHistory();
  const [cardPayment, setCardPayment] = React.useState('');
  const [status, setStatus] = React.useState('');
  const [bank, setBank] = React.useState(props.data.bank);
  const [directoryServer, setDirectoryServer] = React.useState();
  const [values, setValues] = React.useState({
    address: '',
    cardNumber: '',
    code: ''
  });
  const [open, setOpen] = React.useState(false);

  useEffect(() => {
    if (!bank) {
      fetchBank();
    } else if (!directoryServer) {
      fetchDirectoryServer();
    }
  });

  async function fetchBank() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/banks', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    const data = await response.json();
    setBank(data)
  }

  async function fetchDirectoryServer() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/directoryServers', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    const data = await response.json();
    setDirectoryServer(data);
  }

  const handleChange = (event) => {
    setCardPayment(event.target.value);
  };

  const handleInputChange = (event) => {
    setValues({
      ...values,
      [event.target.name]: event.target.value,
    });
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleToggle = () => {
    setOpen(!open);
  };

  const addOrder = () => {
    if (bank) {
      axios.post('api/orders/postOrder', {
        price: props.data.basketPrice,
        cardNumber: values.cardNumber,
        items: props.data.basket.map(product => ({
          product: product
        }))
      })
        .then(function (response) {
          props.handleBasketReset();

          window.location =
            bank.url + bank.path + '?orderId=' + response.data.id
            + '&apiKey=' + bank.apiKey
            + '&cardNumber=' + values.cardNumber;
        })
        .catch(function (error) {
          handleClose();
          setStatus(error);
        })
    } else {
      history.push('/')
    }
  }

  const handleSumbit = () => {
    handleToggle();

    if (directoryServer) {
      axios.post(directoryServer.url + directoryServer.path, {
        apiKey: directoryServer.apiKey,
        cardNumber: values.cardNumber,
        code: values.code
      })
        .then(function (response) {
          if (response.data.status) {
            addOrder();
          } else {
            handleClose();
            setStatus(response.data.status);
          }
        })
        .catch(function (error) {
          handleClose();
          console.log(error)
          if (error.response) {
            setStatus(error.response.status);
          } else {
            setStatus(error);
          }
        })
    } else {
      history.push('/')
    }
  }

  let button = values.address.length > 5 &&
    values.cardNumber.trim().length === 19 &&
    values.code.trim().length === 4 && props.data.basketPrice > 0 ? (
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
      <Backdrop className={classes.backdrop} open={open} onClick={handleClose}>
        <CircularProgress color="inherit" />
      </Backdrop>
    </div>
  );
}