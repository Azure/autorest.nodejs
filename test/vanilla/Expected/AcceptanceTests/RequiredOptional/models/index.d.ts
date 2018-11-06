/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for
 * license information.
 *
 * Code generated by Microsoft (R) AutoRest Code Generator.
 * Changes may cause incorrect behavior and will be lost if the code is
 * regenerated.
 */

import * as moment from "moment";


/**
 * @class
 * Initializes a new instance of the ErrorModel class.
 * @constructor
 * @property {number} [status]
 * @property {string} [message]
 */
export interface ErrorModel {
  status?: number;
  message?: string;
}

/**
 * @class
 * Initializes a new instance of the IntWrapper class.
 * @constructor
 * @property {number} value
 */
export interface IntWrapper {
  value: number;
}

/**
 * @class
 * Initializes a new instance of the IntOptionalWrapper class.
 * @constructor
 * @property {number} [value]
 */
export interface IntOptionalWrapper {
  value?: number;
}

/**
 * @class
 * Initializes a new instance of the StringWrapper class.
 * @constructor
 * @property {string} value
 */
export interface StringWrapper {
  value: string;
}

/**
 * @class
 * Initializes a new instance of the StringOptionalWrapper class.
 * @constructor
 * @property {string} [value]
 */
export interface StringOptionalWrapper {
  value?: string;
}

/**
 * @class
 * Initializes a new instance of the ArrayWrapper class.
 * @constructor
 * @property {array} value
 */
export interface ArrayWrapper {
  value: string[];
}

/**
 * @class
 * Initializes a new instance of the ArrayOptionalWrapper class.
 * @constructor
 * @property {array} [value]
 */
export interface ArrayOptionalWrapper {
  value?: string[];
}

/**
 * @class
 * Initializes a new instance of the Product class.
 * @constructor
 * @property {number} id
 * @property {string} [name]
 */
export interface Product {
  id: number;
  name?: string;
}

/**
 * @class
 * Initializes a new instance of the ClassWrapper class.
 * @constructor
 * @property {object} value
 * @property {number} [value.id]
 * @property {string} [value.name]
 */
export interface ClassWrapper {
  value: Product;
}

/**
 * @class
 * Initializes a new instance of the ClassOptionalWrapper class.
 * @constructor
 * @property {object} [value]
 * @property {number} [value.id]
 * @property {string} [value.name]
 */
export interface ClassOptionalWrapper {
  value?: Product;
}
