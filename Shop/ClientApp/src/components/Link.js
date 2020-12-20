/* eslint-disable jsx-a11y/anchor-is-valid */
import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Link from '@material-ui/core/Link';
import Typography from '@material-ui/core/Typography';

const useStyles = makeStyles((theme) => ({
  root: {
      margin: theme.spacing(2),
  },
}));

export default function Links({ name, link }) {
  const classes = useStyles();

  return (
      <Link className={classes.root} href={link} color="inherit"> 
        {name}
      </Link>
  );
}