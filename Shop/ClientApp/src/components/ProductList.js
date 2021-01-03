import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Accordion from '@material-ui/core/Accordion';
import AccordionSummary from '@material-ui/core/AccordionSummary';
import AccordionDetails from '@material-ui/core/AccordionDetails';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import { Grid } from '@material-ui/core';

const useStyles = makeStyles((theme) => ({
  root: {
    width: '100%',
  },
  heading: {
    fontSize: theme.typography.pxToRem(15),
    fontWeight: theme.typography.fontWeightRegular,
  },
  line: {
    textDecoration: "line-through",
  },
  price: {
    float: "right"
  },
  sale: {
    color: "#4caf50",
  }
}));

export default function SimpleAccordion(props) {
  const classes = useStyles();
  const basket = props.data.basket;

  return (
    <div className={classes.root}>
      {basket.map((product) => {
        return (
          <Accordion key={product}>
            <AccordionSummary
              expandIcon={<ExpandMoreIcon />}
              aria-controls="panel1a-content"
              id="panel1a-header"
            >
              <Typography className={classes.heading}>{product.name}</Typography>
              <Grid
                  item
                  alignItems="center"
                  justify="center"
                >
                  {product.price === product.salePrice ?
                    <Typography variant="h5" >
                      {product.price} zł
                    </Typography>
                    :
                    <div className={classes.price}>
                      <Typography className={classes.line} variant="caption" color="secondary">
                        {product.price} zł
                      </Typography>
                      <Typography className={classes.sale} variant="h5">
                        {product.salePrice} zł
                      </Typography>
                    </div>
                  }
                </Grid>
            </AccordionSummary>
            <AccordionDetails>
              <Grid
                container
                spacing={3}
              >
                <Grid item>
                  <Typography>
                    {product.description}
                  </Typography>
                </Grid>
              </Grid>
            </AccordionDetails>
          </Accordion>
        );
      })}
      <Accordion disabled>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls="panel3a-content"
          id="panel3a-header"
        >
          <Typography className={classes.heading}>Disabled Accordion</Typography>
        </AccordionSummary>
      </Accordion>
    </div>
  );
}