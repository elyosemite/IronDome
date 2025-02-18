import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend, Rate, Counter } from 'k6/metrics';

export let options = {
  stages: [
    { duration: '30s', target: 50 },
    { duration: '1m', target: 100 },
    { duration: '30s', target: 0 }
  ],
  thresholds: {
    http_req_duration: ['p(95)<500'], // 95% das requisições devem ser respondidas abaixo de 500ms
  }
};

let responseTime = new Trend('custom_http_req_duration');
let errorRate = new Rate('errors');
let requestCount = new Counter('request_count');

export default function () {
  let endpoint = __ENV.ENDPOINT || 'http://host.docker.internal/guid';
  let res = http.get(endpoint);

  responseTime.add(res.timings.duration);
  requestCount.add(1);
  errorRate.add(res.status !== 200);

  check(res, {
    'status is 200': (r) => r.status === 200,
    'latência abaixo de 200ms': (r) => r.timings.duration < 200
  });

  sleep(1);
}
