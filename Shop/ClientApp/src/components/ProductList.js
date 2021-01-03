import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Accordion from '@material-ui/core/Accordion';
import AccordionSummary from '@material-ui/core/AccordionSummary';
import AccordionDetails from '@material-ui/core/AccordionDetails';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import { AccordionActions, Grid } from '@material-ui/core';
import { Button } from '@material-ui/core';

const useStyles = makeStyles((theme) => ({
  root: {
    width: '100%',
  },
  heading: {
    fontSize: theme.typography.pxToRem(30),
    fontWeight: theme.typography.fontWeightRegular,
    marginLeft: '15%'
  },
  secondaryHeading: {
    fontSize: theme.typography.pxToRem(20),
    float: 'right',
    marginRight: '15%'
  },
  line: {
    textDecoration: 'line-through',
    float: 'right'
  },
  sale: {
    color: '#4caf50',
    fontSize: theme.typography.pxToRem(22),
  },
  column: {
    flexBasis: '50%',
  },
  delete: {
    fontSize: theme.typography.pxToRem(15),
    float: 'right',
  },
}));

export default function SimpleAccordion(props) {
  const classes = useStyles();
  const basket = props.data.basket;

  const handleProductRemoveClick = (id) => {
    props.handleProductRemoveClick(id);
  };

  return (
    <div className={classes.root}>
      {basket.map((product, index) => {
        return (
          <Accordion
           key={index}
           variant="outlined"
          >
            <AccordionSummary
              expandIcon={<ExpandMoreIcon />}
              aria-controls="panel1a-content"
              id="panel1a-header"
            >
              <div className={classes.column}>
                <Typography
                  className={classes.heading}
                >
                  {product.name}
                </Typography>
              </div>
              <div className={classes.column}>
                {product.price === product.salePrice ?
                  <Typography className={classes.secondaryHeading}>
                    {product.price} zł
                    </Typography>
                  :
                  <div className={classes.secondaryHeading}>
                    <Typography className={classes.line} variant="body2" color="secondary">
                      {product.price} zł
                      </Typography>
                    <Typography className={classes.sale} >
                      {product.salePrice} zł
                      </Typography>
                  </div>
                }
              </div>
              <div className={classes.column}>
                <Typography>
                  <Button 
                    onClick={(event) => event.stopPropagation()}
                    onFocus={(event) => event.stopPropagation()}
                    size="small"
                    color="secondary"
                    variant="contained"
                    className={classes.delete}
                    onClick={() => handleProductRemoveClick(index)}
                  >
                    Usuń
                  </Button>
                </Typography>
              </div>
            </AccordionSummary>
            <AccordionDetails>
              <div className={classes.column}>
                <Typography>
                  {product.description}
                </Typography>
              </div>
            </AccordionDetails>
          </Accordion>
        );
      })}
    </div>
  );
}