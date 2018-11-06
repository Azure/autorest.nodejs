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

/**
 * Class representing a Product.
 */
class Product {
  /**
   * Create a Product.
   * @property {number} [integer]
   * @property {string} [string]
   */
  constructor() {
  }

  /**
   * Defines the metadata of Product
   *
   * @returns {object} metadata of Product
   *
   */
  mapper() {
    return {
      required: false,
      serializedName: 'Product',
      type: {
        name: 'Composite',
        className: 'Product',
        modelProperties: {
          integer: {
            required: false,
            serializedName: 'integer',
            type: {
              name: 'Number'
            }
          },
          string: {
            required: false,
            serializedName: 'string',
            type: {
              name: 'String'
            }
          }
        }
      }
    };
  }
}

module.exports = Product;
