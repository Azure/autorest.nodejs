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
 * Additional parameters for a set of operations, such as:
 * ParameterGrouping_postMultiParamGroups,
 * ParameterGrouping_postSharedParameterGroupObject.
 *
 */
class FirstParameterGroup {
  /**
   * Create a FirstParameterGroup.
   * @property {string} [headerOne]
   * @property {number} [queryOne] Query parameter with default. Default value:
   * 30 .
   */
  constructor() {
  }

  /**
   * Defines the metadata of FirstParameterGroup
   *
   * @returns {object} metadata of FirstParameterGroup
   *
   */
  mapper() {
    return {
      required: false,
      type: {
        name: 'Composite',
        className: 'FirstParameterGroup',
        modelProperties: {
          headerOne: {
            required: false,
            type: {
              name: 'String'
            }
          },
          queryOne: {
            required: false,
            defaultValue: 30,
            type: {
              name: 'Number'
            }
          }
        }
      }
    };
  }
}

module.exports = FirstParameterGroup;
