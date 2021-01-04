import React from 'react';
import { Grid } from '@material-ui/core';
import Cards from './Card';

export default function ProductCars(props) {
  return (
    <Grid
      container justify="center" spacing={3}>
      {props.products.map((item) => (
        <Grid
          key={item.id} item>
          <Cards product={item} handleProductAddClick={props.handleProductAddClick} />
        </Grid>
      ))}
    </Grid>
  );
}