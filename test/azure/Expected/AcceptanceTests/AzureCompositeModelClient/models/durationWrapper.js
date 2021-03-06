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
 * Class representing a DurationWrapper.
 */
class DurationWrapper {
  /**
   * Create a DurationWrapper.
   * @property {moment.duration} [field]
   */
  constructor() {
  }

  /**
   * Defines the metadata of DurationWrapper
   *
   * @returns {object} metadata of DurationWrapper
   *
   */
  mapper() {
    return {
      required: false,
      serializedName: 'duration-wrapper',
      type: {
        name: 'Composite',
        className: 'DurationWrapper',
        modelProperties: {
          field: {
            required: false,
            serializedName: 'field',
            type: {
              name: 'TimeSpan'
            }
          }
        }
      }
    };
  }
}

module.exports = DurationWrapper;
