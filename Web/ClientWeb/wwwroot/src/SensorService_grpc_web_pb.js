/**
 * @fileoverview gRPC-Web generated client stub for sensorsystem
 * @enhanceable
 * @public
 */

// GENERATED CODE -- DO NOT EDIT!


/* eslint-disable */
// @ts-nocheck



const grpc = {};
grpc.web = require('grpc-web');


var google_protobuf_empty_pb = require('google-protobuf/google/protobuf/empty_pb.js')
const proto = {};
proto.sensorsystem = require('./SensorService_pb.js');

/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?Object} options
 * @constructor
 * @struct
 * @final
 */
proto.sensorsystem.SensorServiceClient =
    function(hostname, credentials, options) {
  if (!options) options = {};
  options['format'] = 'text';

  /**
   * @private @const {!grpc.web.GrpcWebClientBase} The client
   */
  this.client_ = new grpc.web.GrpcWebClientBase(options);

  /**
   * @private @const {string} The hostname
   */
  this.hostname_ = hostname;

};


/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?Object} options
 * @constructor
 * @struct
 * @final
 */
proto.sensorsystem.SensorServicePromiseClient =
    function(hostname, credentials, options) {
  if (!options) options = {};
  options['format'] = 'text';

  /**
   * @private @const {!grpc.web.GrpcWebClientBase} The client
   */
  this.client_ = new grpc.web.GrpcWebClientBase(options);

  /**
   * @private @const {string} The hostname
   */
  this.hostname_ = hostname;

};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.sensorsystem.AvailableSensorsRequest,
 *   !proto.sensorsystem.AvailableSensorsResponse>}
 */
const methodDescriptor_SensorService_GetAvailableSensors = new grpc.web.MethodDescriptor(
  '/sensorsystem.SensorService/GetAvailableSensors',
  grpc.web.MethodType.UNARY,
  proto.sensorsystem.AvailableSensorsRequest,
  proto.sensorsystem.AvailableSensorsResponse,
  /**
   * @param {!proto.sensorsystem.AvailableSensorsRequest} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  proto.sensorsystem.AvailableSensorsResponse.deserializeBinary
);


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.sensorsystem.AvailableSensorsRequest,
 *   !proto.sensorsystem.AvailableSensorsResponse>}
 */
const methodInfo_SensorService_GetAvailableSensors = new grpc.web.AbstractClientBase.MethodInfo(
  proto.sensorsystem.AvailableSensorsResponse,
  /**
   * @param {!proto.sensorsystem.AvailableSensorsRequest} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  proto.sensorsystem.AvailableSensorsResponse.deserializeBinary
);


/**
 * @param {!proto.sensorsystem.AvailableSensorsRequest} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.sensorsystem.AvailableSensorsResponse)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.sensorsystem.AvailableSensorsResponse>|undefined}
 *     The XHR Node Readable Stream
 */
proto.sensorsystem.SensorServiceClient.prototype.getAvailableSensors =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/sensorsystem.SensorService/GetAvailableSensors',
      request,
      metadata || {},
      methodDescriptor_SensorService_GetAvailableSensors,
      callback);
};


/**
 * @param {!proto.sensorsystem.AvailableSensorsRequest} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.sensorsystem.AvailableSensorsResponse>}
 *     Promise that resolves to the response
 */
proto.sensorsystem.SensorServicePromiseClient.prototype.getAvailableSensors =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/sensorsystem.SensorService/GetAvailableSensors',
      request,
      metadata || {},
      methodDescriptor_SensorService_GetAvailableSensors);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.sensorsystem.TemperatureRequest,
 *   !proto.sensorsystem.TemperatureData>}
 */
const methodDescriptor_SensorService_ReceiveTemperatureUpdates = new grpc.web.MethodDescriptor(
  '/sensorsystem.SensorService/ReceiveTemperatureUpdates',
  grpc.web.MethodType.SERVER_STREAMING,
  proto.sensorsystem.TemperatureRequest,
  proto.sensorsystem.TemperatureData,
  /**
   * @param {!proto.sensorsystem.TemperatureRequest} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  proto.sensorsystem.TemperatureData.deserializeBinary
);


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.sensorsystem.TemperatureRequest,
 *   !proto.sensorsystem.TemperatureData>}
 */
const methodInfo_SensorService_ReceiveTemperatureUpdates = new grpc.web.AbstractClientBase.MethodInfo(
  proto.sensorsystem.TemperatureData,
  /**
   * @param {!proto.sensorsystem.TemperatureRequest} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  proto.sensorsystem.TemperatureData.deserializeBinary
);


/**
 * @param {!proto.sensorsystem.TemperatureRequest} request The request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.sensorsystem.TemperatureData>}
 *     The XHR Node Readable Stream
 */
proto.sensorsystem.SensorServiceClient.prototype.receiveTemperatureUpdates =
    function(request, metadata) {
  return this.client_.serverStreaming(this.hostname_ +
      '/sensorsystem.SensorService/ReceiveTemperatureUpdates',
      request,
      metadata || {},
      methodDescriptor_SensorService_ReceiveTemperatureUpdates);
};


/**
 * @param {!proto.sensorsystem.TemperatureRequest} request The request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.sensorsystem.TemperatureData>}
 *     The XHR Node Readable Stream
 */
proto.sensorsystem.SensorServicePromiseClient.prototype.receiveTemperatureUpdates =
    function(request, metadata) {
  return this.client_.serverStreaming(this.hostname_ +
      '/sensorsystem.SensorService/ReceiveTemperatureUpdates',
      request,
      metadata || {},
      methodDescriptor_SensorService_ReceiveTemperatureUpdates);
};


module.exports = proto.sensorsystem;

