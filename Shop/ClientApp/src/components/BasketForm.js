import { Button, FormControl, FormHelperText, Grid, InputLabel, makeStyles, MenuItem, Paper, Select, Typography } from '@material-ui/core';
import React from 'react';
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
    width: "60%"
  }
}));

export default function BasketForm(props) {
  const classes = useStyles();
  const [cardPayment, setCardPayment] = React.useState('');

  let history = useHistory();

  const handleChange = (event) => {
    setCardPayment(event.target.value);
  };

  const handleSumbit = () => {
    if(!cardPayment){
      props.handleSetPayment(true);
      history.push('/summary/cashOnDelivery')
    }
  }

  var button = props.data.itemsCount > 0 && cardPayment !== '' ? (
    <Button
      variant="contained"
      color="primary"
      onClick={handleSumbit}
    >
      Złóż zamówienie
    </Button>
  ) : (
      <Button
        variant="contained"
        color="primary"
        disabled
      >
        Złóż zamówienie
      </Button>
    )

  return (
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
                {props.data.basketPrice} zł
              </Typography>
            </Paper>
          </Grid>
          <Grid item xs={12}>
            <FormControl className={classes.formControl}>
              <InputLabel id="select-helper-label">Płatność</InputLabel>
              <Select
                labelId="select-helper-label"
                id="select-helper"
                value={cardPayment}
                onChange={handleChange}
              >
                <MenuItem value={true} selected>Kartą</MenuItem>
                <MenuItem value={false}>Przy odbiorze</MenuItem>
              </Select>
              <FormHelperText>Wybierz rodzaj płatności</FormHelperText>
            </FormControl>
          </Grid>
          <Grid item xs={8}>
            {button}
          </Grid>
        </Grid>
      </Paper>
    </Paper>
  );
}