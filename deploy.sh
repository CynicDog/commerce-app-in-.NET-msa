#!/bin/bash

# Get the directory of the script
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Set the paths to your Dockerfiles
PRODUCT_CATALOG_DOCKERFILE="$SCRIPT_DIR/ProductCatalogService/Dockerfile"
SHOPPING_CART_DOCKERFILE="$SCRIPT_DIR/ShoppingCartService/Dockerfile"

# Function to build Docker images
build_images() {
  echo "Building ProductCatalogService image..."
  docker build -t product-catalog -f $PRODUCT_CATALOG_DOCKERFILE "$SCRIPT_DIR/ProductCatalogService"

  echo "Building ShoppingCartService image..."
  docker build -t shopping-cart -f $SHOPPING_CART_DOCKERFILE "$SCRIPT_DIR/ShoppingCartService"
}

# Set the paths to your Kubernetes manifest files
PRODUCT_CATALOG_MANIFEST="$SCRIPT_DIR/ProductCatalogService"
SHOPPING_CART_MANIFEST="$SCRIPT_DIR/ShoppingCartService/Deployment"

# Function to deploy Kubernetes manifests
deploy_manifests() {
  echo "Deploying ProductCatalogService manifest..."
  kubectl apply -f $PRODUCT_CATALOG_MANIFEST

  echo "Deploying ShoppingCartService manifest..."
  kubectl apply -f $SHOPPING_CART_MANIFEST
}

# Main script execution
build_images

deploy_manifests

echo "Deployment completed successfully!"
