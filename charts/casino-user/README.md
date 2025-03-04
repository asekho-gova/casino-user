# Casino User API Helm Chart

This Helm chart deploys the Casino User API application on a Kubernetes cluster.

## Prerequisites

- Kubernetes 1.19+
- Helm 3.2.0+

## Installing the Chart

To install the chart with the release name `my-release`:

```bash
helm install my-release ./casino-user
```

## Parameters

### Global parameters

| Name                      | Description                                     | Value |
| ------------------------- | ----------------------------------------------- | ----- |
| `global.imageRegistry`    | Global Docker image registry                    | `""`  |
| `global.imagePullSecrets` | Global Docker registry secret names as an array | `[]`  |

### Common parameters

| Name                | Description                                                                                  | Value |
| ------------------- | -------------------------------------------------------------------------------------------- | ----- |
| `nameOverride`      | String to partially override the fullname template (will maintain the release name)          | `""`  |
| `fullnameOverride`  | String to fully override the fullname template                                               | `""`  |
| `podAnnotations`    | Annotations for pods                                                                         | `{}`  |
| `podSecurityContext`| Security context for pods                                                                    | `{}`  |
| `securityContext`   | Security context for containers                                                              | `{}`  |

### Casino User API parameters

| Name                                 | Description                                                          | Value                 |
| ------------------------------------ | -------------------------------------------------------------------- | --------------------- |
| `image.repository`                   | Casino User API image repository                                     | `casino-user`         |
| `image.tag`                          | Casino User API image tag                                            | `latest`              |
| `image.pullPolicy`                   | Casino User API image pull policy                                    | `IfNotPresent`        |
| `replicaCount`                       | Number of Casino User API replicas                                   | `1`                   |
| `resources.limits.cpu`               | The CPU limits for the Casino User API container                     | `500m`                |
| `resources.limits.memory`            | The memory limits for the Casino User API container                  | `512Mi`               |
| `resources.requests.cpu`             | The requested CPU for the Casino User API container                  | `100m`                |
| `resources.requests.memory`          | The requested memory for the Casino User API container               | `128Mi`               |

### Service parameters

| Name                               | Description                                                      | Value       |
| ---------------------------------- | ---------------------------------------------------------------- | ----------- |
| `service.type`                     | Casino User API service type                                     | `ClusterIP` |
| `service.port`                     | Casino User API service port                                     | `80`        |
| `service.targetPort`               | Casino User API container port                                   | `8080`      |

### Ingress parameters

| Name                       | Description                                                          | Value                    |
| -------------------------- | -------------------------------------------------------------------- | ------------------------ |
| `ingress.enabled`          | Enable ingress record generation for Casino User API                 | `false`                  |
| `ingress.className`        | IngressClass that will be be used to implement the Ingress           | `""`                     |
| `ingress.annotations`      | Additional annotations for the Ingress resource                      | `{}`                     |
| `ingress.hosts`            | An array with hosts and paths for the Ingress                        | See values.yaml for details |
| `ingress.tls`              | TLS configuration for the Ingress                                    | `[]`                     |

### Autoscaling parameters

| Name                                      | Description                                                                            | Value   |
| ----------------------------------------- | -------------------------------------------------------------------------------------- | ------- |
| `autoscaling.enabled`                     | Enable autoscaling for Casino User API                                                 | `false` |
| `autoscaling.minReplicas`                 | Minimum number of Casino User API replicas                                             | `1`     |
| `autoscaling.maxReplicas`                 | Maximum number of Casino User API replicas                                             | `10`    |
| `autoscaling.targetCPUUtilizationPercentage` | Target CPU utilization percentage                                                   | `80`    |
| `autoscaling.targetMemoryUtilizationPercentage` | Target Memory utilization percentage                                             | `""`    |
