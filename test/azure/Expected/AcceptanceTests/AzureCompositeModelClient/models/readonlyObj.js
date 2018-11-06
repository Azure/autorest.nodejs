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
 * Class representing a ReadonlyObj.
 */
class ReadonlyObj {
  /**
   * Create a ReadonlyObj.
   * @property {string} [id]
   * @property {number} [size]
   */
  constructor() {
  }

  /**
   * Defines the metadata of ReadonlyObj
   *
   * @returns {object} metadata of ReadonlyObj
   *
   */
  mapper() {
    return {
      required: false,
      serializedName: 'readonly-obj',
      type: {
        name: 'Composite',
        className: 'ReadonlyObj',
        modelProperties: {
          id: {
            required: false,
            readOnly: true,
            serializedName: 'id',
            type: {
              name: 'String'
            }
          },
          size: {
            required: false,
            serializedName: 'size',
            type: {
              name: 'Number'
            }
          }
        }
      }
    };
  }
}

module.exports = ReadonlyObj;
