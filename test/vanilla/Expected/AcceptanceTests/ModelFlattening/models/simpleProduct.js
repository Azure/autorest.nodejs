/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for
 * license information.
 *
 * Code generated by Microsoft (R) AutoRest Code Generator.
 * Changes may cause incorrect behavior and will be lost if the code is
 * regenerated.
 */

'use strict';

const models = require('./index');

/**
 * The product documentation.
 *
 * @extends models['BaseProduct']
 */
class SimpleProduct extends models['BaseProduct'] {
  /**
   * Create a SimpleProduct.
   * @property {string} maxProductDisplayName Display name of product.
   * @property {string} [genericValue] Generic URL value.
   * @property {string} [odatavalue] URL value.
   */
  constructor() {
    super();
  }

  /**
   * Defines the metadata of SimpleProduct
   *
   * @returns {object} metadata of SimpleProduct
   *
   */
  mapper() {
    return {
      required: false,
      serializedName: 'SimpleProduct',
      type: {
        name: 'Composite',
        className: 'SimpleProduct',
        modelProperties: {
          productId: {
            required: true,
            serializedName: 'base_product_id',
            type: {
              name: 'String'
            }
          },
          description: {
            required: false,
            serializedName: 'base_product_description',
            type: {
              name: 'String'
            }
          },
          maxProductDisplayName: {
            required: true,
            serializedName: 'details.max_product_display_name',
            type: {
              name: 'String'
            }
          },
          capacity: {
            required: true,
            isConstant: true,
            serializedName: 'details.max_product_capacity',
            defaultValue: 'Large',
            type: {
              name: 'String'
            }
          },
          genericValue: {
            required: false,
            serializedName: 'details.max_product_image.generic_value',
            type: {
              name: 'String'
            }
          },
          odatavalue: {
            required: false,
            serializedName: 'details.max_product_image.@odata\\.value',
            type: {
              name: 'String'
            }
          }
        }
      }
    };
  }
}

module.exports = SimpleProduct;
