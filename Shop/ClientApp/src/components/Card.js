import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import clsx from 'clsx';
import Card from '@material-ui/core/Card';
import CardHeader from '@material-ui/core/CardHeader';
import CardMedia from '@material-ui/core/CardMedia';
import CardContent from '@material-ui/core/CardContent';
import CardActions from '@material-ui/core/CardActions';
import Collapse from '@material-ui/core/Collapse';
import Avatar from '@material-ui/core/Avatar';
import IconButton from '@material-ui/core/IconButton';
import Typography from '@material-ui/core/Typography';
import { red } from '@material-ui/core/colors';
import FavoriteIcon from '@material-ui/icons/Favorite';
import ShareIcon from '@material-ui/icons/Share';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import MoreVertIcon from '@material-ui/icons/MoreVert';
import Icon from '@material-ui/core/Icon';
import AddShoppingCartIcon from '@material-ui/icons/AddShoppingCart';

const useStyles = makeStyles((theme) => ({
  root: {
    height: 405,
    width: 345,
  },
  media: {
    height: 0,
    paddingTop: '56.25%', // 16:9
  },
  add: {
    marginLeft: 'auto',
  },
  avatar: {
    backgroundColor: red[500],
  },
  content: {
    maxHeight: 54,
  },
  line: {
    textDecoration: "line-through",
  },
}));

export default function RecipeReviewCard({ product }) {
  const classes = useStyles();
  const [added, setAdded] = React.useState(false);

  const handleAddClick = () => {
    setAdded(!added);
  };

  return (
    <Card className={classes.root}>
      <CardHeader
        titleTypographyProps={{ variant: 'h6' }}
        avatar={
          <Avatar aria-label="recipe" className={classes.avatar}>
            {product.name.charAt(0)}
          </Avatar>
        }
        title={product.name}
      />
      <CardMedia
        className={classes.media}
        image={"images/" + product.pictures[0].path}
        title={product.name}
      />
      <CardContent
        className={classes.content}
      >
        <Typography variant="body2" color="textSecondary" component="p">
          {product.description}
        </Typography>
      </CardContent>
      <CardActions disableSpacing>
        {product.price === product.salePrice ?
          <Typography variant="h5" >
            {product.price} zł
          </Typography>
          :
          <div>
            <Typography  className={classes.line} variant="caption" color="secondary">
              {product.price} zł
            </Typography>
            <Typography variant="h5" color="success">
              {product.salePrice} zł
            </Typography>
          </div>
        }
        <IconButton
          color="primary"
          aria-label="add to shopping cart"
          className={classes.add}
          onClick={handleAddClick}
        >
          <AddShoppingCartIcon fontSize="large" />
        </IconButton>
      </CardActions>
    </Card>
  );
}