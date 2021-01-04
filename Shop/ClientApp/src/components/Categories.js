import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Chip from '@material-ui/core/Chip';
import Paper from '@material-ui/core/Paper';
import Skeleton from '@material-ui/lab/Skeleton';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    justifyContent: 'center',
    flexWrap: 'wrap',
    listStyle: 'none',
    padding: theme.spacing(0.5),
    margin: 0,
    backgroundColor: "#3d3d3d",
  },
  chip: {
    fontSize: 30,
    margin: theme.spacing(0.5),
  },
}));

export default function Categories(props) {
  const classes = useStyles();

  const handleClick = (id) => {
    props.handleCategoriesClick(id);
  };

  const skeleton = [0, 1, 2, 4];

  return (
    <Paper
      component="ul"
      className={classes.root}
    >
      <Chip
        label={"Wszystkie"}
        onClick={() => handleClick(0)}
        className={classes.chip}
      />
      { props.categoriesLoading ?
        skeleton.map((index) => {
          return (
            <li key={index}>
              <Skeleton className={classes.chip} variant="circle" width={100} height={40} />
            </li>
          );
        })
        :
        props.categoriesList.map((data) => {
          return (
            <li key={data.id}>
              <Chip
                label={data.name}
                onClick={() => handleClick(data.id)}
                className={classes.chip}
              />
            </li>
          );
        })
      }
    </Paper>
  );
}