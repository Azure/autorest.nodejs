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
 * Class representing a SampleResourceGroup.
 */
class SampleResourceGroup {
  /**
   * Create a SampleResourceGroup.
   * @property {string} [name] resource group name 'testgroup101'
   * @property {string} [location] resource group location 'West US'
   */
  constructor() {
  }

  /**
   * Defines the metadata of SampleResourceGroup
   *
   * @returns {object} metadata of SampleResourceGroup
   *
   */
  mapper() {
    return {
      required: false,
      serializedName: 'SampleResourceGroup',
      type: {
        name: 'Composite',
        className: 'SampleResourceGroup',
        modelProperties: {
          name: {
            required: false,
            serializedName: 'name',
            type: {
              name: 'String'
            }
          },
          location: {
            required: false,
            serializedName: 'location',
            type: {
              name: 'String'
            }
          }
        }
      }
    };
  }
}

module.exports = SampleResourceGroup;
