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
 * Class representing a ResourceCollection.
 */
class ResourceCollection {
  /**
   * Create a ResourceCollection.
   * @member {object} [productresource]
   * @member {string} [productresource.pname]
   * @member {string} [productresource.flattenedProductType]
   * @member {string} [productresource.provisioningStateValues] Possible values
   * include: 'Succeeded', 'Failed', 'canceled', 'Accepted', 'Creating',
   * 'Created', 'Updating', 'Updated', 'Deleting', 'Deleted', 'OK'
   * @member {string} [productresource.provisioningState]
   * @member {array} [arrayofresources]
   * @member {object} [dictionaryofresources]
   */
  constructor() {
  }

  /**
   * Defines the metadata of ResourceCollection
   *
   * @returns {object} metadata of ResourceCollection
   *
   */
  mapper() {
    return {
      required: false,
      serializedName: 'ResourceCollection',
      type: {
        name: 'Composite',
        className: 'ResourceCollection',
        modelProperties: {
          productresource: {
            required: false,
            serializedName: 'productresource',
            type: {
              name: 'Composite',
              className: 'FlattenedProduct'
            }
          },
          arrayofresources: {
            required: false,
            serializedName: 'arrayofresources',
            type: {
              name: 'Sequence',
              element: {
                  required: false,
                  serializedName: 'FlattenedProductElementType',
                  type: {
                    name: 'Composite',
                    className: 'FlattenedProduct'
                  }
              }
            }
          },
          dictionaryofresources: {
            required: false,
            serializedName: 'dictionaryofresources',
            type: {
              name: 'Dictionary',
              value: {
                  required: false,
                  serializedName: 'FlattenedProductElementType',
                  type: {
                    name: 'Composite',
                    className: 'FlattenedProduct'
                  }
              }
            }
          }
        }
      }
    };
  }
}

module.exports = ResourceCollection;
