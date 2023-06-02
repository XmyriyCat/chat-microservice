FROM node:18.16.0-alpine

ENV WDS_SOCKET_PORT=443

WORKDIR /app
COPY . .
RUN npm run build
CMD ["npm", "start"]