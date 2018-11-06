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
 * The product documentation.
 *
 */
class ChildProduct {
  /**
   * Create a ChildProduct.
   * @property {number} [count] Count
   */
  constructor() {
  }

  /**
   * Defines the metadata of ChildProduct
   *
   * @returns {object} metadata of ChildProduct
   *
   */
  mapper() {
    return {
      required: false,
      serializedName: 'ChildProduct',
      type: {
        name: 'Composite',
        className: 'ChildProduct',
        modelProperties: {
          constProperty: {
            required: true,
            isConstant: true,
            serializedName: 'constProperty',
            defaultValue: 'constant',
            type: {
              name: 'String'
            }
          },
          count: {
            required: false,
            serializedName: 'count',
            type: {
              name: 'Number'
            }
          }
        }
      }
    };
  }
}

module.exports = ChildProduct;
