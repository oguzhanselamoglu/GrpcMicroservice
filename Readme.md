# Grpc usage for microservice
There are three Grpc servers which are SoppingCartGrpc, ProductGrpc and DiscountGrpc

# ProductGrpc

ProductGrpc server is Asp.Net grpc server and expose apis for Product Crud operations

# ProductWorkerService
to consuming productGrp service. Worker service will be client of ProductGrpc and generate product and insert bulk operation

# SoppingCartGrpc
SoppingCartGrpc is Asp.Net grpc server and expose Shopping cart items operations.

# ShopCartWorkerService
to consuming ShoppingCartgrpc server. Worker service will be client of both ProductGrpc and ShoppingCartGrpc servers. this worker will read the product from ProductGrpc server and create shopping cart item using ShoppingCartGrpc server
this operations will be in a time interval and looping as s service application


