/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 *
 * Code generated by Microsoft (R) AutoRest Code Generator.
 * Changes may cause incorrect behavior and will be lost if the code is regenerated.
 */

import * as moment from "moment";

export interface ErrorModel {
  status?: number;
  message?: string;
  parentError?: ErrorModel;
}

export interface Resource {
  /**
   * Resource Id
  */
  readonly id?: string;
  /**
   * Resource Type
  */
  readonly type?: string;
  tags?: { [propertyName: string]: string };
  /**
   * Resource Location
  */
  location?: string;
  /**
   * Resource Name
  */
  readonly name?: string;
}

/**
 * Flattened product.
*/
export interface FlattenedProduct extends Resource {
  pname?: string;
  flattenedProductType?: string;
  /**
   * Possible values include: 'Succeeded', 'Failed', 'canceled', 'Accepted', 'Creating', 'Created',
   * 'Updating', 'Updated', 'Deleting', 'Deleted', 'OK'
  */
  readonly provisioningStateValues?: string;
  provisioningState?: string;
}

export interface ResourceCollection {
  productresource?: FlattenedProduct;
  arrayofresources?: FlattenedProduct[];
  dictionaryofresources?: { [propertyName: string]: FlattenedProduct };
}

/**
 * The product documentation.
*/
export interface BaseProduct {
  /**
   * Unique identifier representing a specific product for a given latitude & longitude. For
   * example, uberX in San Francisco will have a different product_id than uberX in Los Angeles.
  */
  productId: string;
  /**
   * Description of product.
  */
  description?: string;
}

/**
 * The product documentation.
*/
export interface SimpleProduct extends BaseProduct {
  /**
   * Display name of product.
  */
  maxProductDisplayName: string;
  /**
   * Generic URL value.
  */
  genericValue?: string;
  /**
   * URL value.
  */
  odatavalue?: string;
}

/**
 * The Generic URL.
*/
export interface GenericUrl {
  /**
   * Generic URL value.
  */
  genericValue?: string;
}

/**
 * The wrapped produc.
*/
export interface WrappedProduct {
  /**
   * the product value
  */
  value?: string;
}

/**
 * The wrapped produc.
*/
export interface ProductWrapper {
  /**
   * the product value
  */
  value?: string;
}

/**
 * Additional parameters for putSimpleProductWithGrouping operation.
*/
export interface FlattenParameterGroup {
  /**
   * Product name with value 'groupproduct'
  */
  name: string;
  /**
   * Unique identifier representing a specific product for a given latitude & longitude. For
   * example, uberX in San Francisco will have a different product_id than uberX in Los Angeles.
  */
  productId: string;
  /**
   * Description of product.
  */
  description?: string;
  /**
   * Display name of product.
  */
  maxProductDisplayName: string;
  /**
   * Generic URL value.
  */
  genericValue?: string;
  /**
   * URL value.
  */
  odatavalue?: string;
}
